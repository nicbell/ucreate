using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Helpers
{
    public class DocTypeHelper : BaseContentTypeHelper<DocTypeAttribute>
    {
        public DocTypeHelper() { }


        public override IContentTypeComposition GetByAlias(string alias)
        {
            return Service.GetContentType(alias);
        }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public override void Save(Type itemType)
        {
            var contentTypes = Service.GetAllContentTypes();
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is DocTypeAttribute) as DocTypeAttribute;
            var ct = contentTypes.FirstOrDefault(x => x.Key == new Guid(attr.Key)) ?? new ContentType(-1) { Key = new Guid(attr.Key) };

            ct.Name = attr.Name;
            ct.Alias = attr.Alias;
            ct.Icon = attr.Icon;
            ct.AllowedAsRoot = attr.AllowedAsRoot;
            ct.IsContainer = attr.IsContainer;
            ct.ParentId = -1;

            SetParent(ct, itemType);
            MapProperties(ct, itemType);
            SetTemplates(ct, attr.AllowedTemplates, attr.DefaultTemplate); //

            Service.Save(ct);
        }


        /// <summary>
        /// Saves allowed types
        /// </summary>
        /// <param name="itemType"></param>
        public override void SaveAllowedTypes(Type itemType)
        {
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is DocTypeAttribute) as DocTypeAttribute;
            var ct = GetByAlias(attr.Alias) as IContentType;

            MapAllowedTypes(ct, attr.AllowedTypes);

            Service.Save(ct);
        }


        /// <summary>
        /// Set templates
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="attr"></param>
        private void SetTemplates(IContentType ct, string[] allowedTemplates, string defaultTemplate)
        {
            if (allowedTemplates == null || allowedTemplates.Length == 0)
                return;

            if (String.IsNullOrEmpty(defaultTemplate))
                defaultTemplate = allowedTemplates.First();


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
    }
}
