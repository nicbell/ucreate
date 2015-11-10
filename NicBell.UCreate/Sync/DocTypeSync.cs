using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Sync
{
    public class DocTypeSync : BaseTreeContentTypeSync<DocTypeAttribute>
    {
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
            var ct = contentTypes.FirstOrDefault(x => x.Alias == itemType.Name) ?? new ContentType(-1);

            ct.Name = attr.Name;
            ct.Alias = itemType.Name;
            ct.Icon = attr.Icon;
            ct.AllowedAsRoot = attr.AllowedAsRoot;
            ct.IsContainer = attr.IsContainer;
            ct.ParentId = -1;

            SetParent(ct, itemType);
            MapProperties(ct, itemType);
            SetTemplates(ct, itemType, attr.AllowedTemplates, attr.DefaultTemplate);

            Service.Save(ct);
        }


        /// <summary>
        /// Set templates for doctypes
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="itemType"></param>
        /// <param name="allowedTemplates"></param>
        /// <param name="defaultTemplate"></param>
        private void SetTemplates(IContentType ct, Type itemType, string[] allowedTemplates, string defaultTemplate)
        {
            if (allowedTemplates == null || allowedTemplates.Length == 0)
                return;

            if (string.IsNullOrEmpty(defaultTemplate))
                defaultTemplate = allowedTemplates.First();


            var templates = new List<ITemplate>();

            foreach (var templateAlias in allowedTemplates)
            {
                var template = GetOrCreateTemplate(itemType, templateAlias);
                templates.Add(template);
            }

            ct.AllowedTemplates = templates;
            ct.SetDefaultTemplate(templates.First(x => x.Alias == defaultTemplate));
        }


        /// <summary>
        /// Creates a template
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="templateAlias"></param>
        /// <returns></returns>
        private ITemplate GetOrCreateTemplate(Type itemType, string templateAlias)
        {
            var template = ApplicationContext.Current.Services.FileService.GetTemplate(templateAlias);

            if (template == null)
            {
                template = new Template(templateAlias, templateAlias);
                //TODO: read file system for existing template.
                template.Content = "@inherits UmbracoTemplatePage<" + itemType.FullName + ">\n\n<h1>@Model.Content.Name</h1>";

                ApplicationContext.Current.Services.FileService.SaveTemplate(template);
                template = ApplicationContext.Current.Services.FileService.GetTemplate(templateAlias);
            }

            return template;
        }
    }
}
