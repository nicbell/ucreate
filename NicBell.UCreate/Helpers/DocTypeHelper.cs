using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Helpers
{
    public class DocTypeHelper : BaseContentTypeHelper
    {
        public DocTypeHelper() { }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public void Save(Type itemType)
        {
            var contentTypes = Service.GetAllContentTypes();
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is DocTypeAttribute) as DocTypeAttribute;

            if (!contentTypes.Any(x => x.Key == new Guid(attr.Key)) || attr.Overwrite)
            {
                var instance = Activator.CreateInstance(itemType, null);
                var ct = contentTypes.FirstOrDefault(x => x.Key == new Guid(attr.Key)) ?? new ContentType(-1) { Key = new Guid(attr.Key) };

                ct.Name = attr.Name;
                ct.Alias = attr.Alias;
                ct.Icon = attr.Icon;
                ct.AllowedAsRoot = attr.AllowedAsRoot;
                ct.IsContainer = attr.IsContainer;

                SetTemplates(ct, attr.AllowedTemplates, attr.DefaultTemplate);
                MapAllowedTypes(ct, attr.AllowedTypes);
                MapProperties(ct, itemType, attr.Overwrite);


                if (instance is IHasPrePostHooks)
                    ((IHasPrePostHooks)instance).PreAdd();

                Service.Save(ct);

                if (instance is IHasPrePostHooks)
                    ((IHasPrePostHooks)instance).PostAdd();
            }
        }


        /// <summary>
        /// Set templates
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="attr"></param>
        private void SetTemplates(IContentType ct, string[] allowedTemplates, string defaultTemplate)
        {
            if (allowedTemplates == null || allowedTemplates.Length == 0 || String.IsNullOrEmpty(defaultTemplate))
                return;

            var templates = new List<ITemplate>();

            foreach (var templateAlias in allowedTemplates)
            {
                var template = ApplicationContext.Current.Services.FileService.GetTemplate(templateAlias);
                if (template == null)
                {
                    ApplicationContext.Current.Services.FileService.SaveTemplate(new Template("-1", templateAlias, templateAlias));
                    template = ApplicationContext.Current.Services.FileService.GetTemplate(templateAlias);
                }
                templates.Add(template);
            }
           
            ct.AllowedTemplates = templates;
            ct.SetDefaultTemplate(templates.First(x => x.Alias == defaultTemplate));
        }


        public override IContentTypeBase GetByAlias(string alias)
        {
            return Service.GetContentType(alias);
        }
    }
}
