﻿using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Sync
{
    public class DocTypeSync : BaseTreeContentTypeSync<DocTypeAttribute>
    {
        /// <summary>
        /// Service
        /// </summary>
        public IContentTypeService Service
        {
            get
            {
                return ApplicationContext.Current.Services.ContentTypeService;
            }
        }


        public override IContentTypeComposition GetByAlias(string alias)
        {
            return Service.GetContentType(alias);
        }


        public override void Save(IContentTypeBase ct)
        {
            Service.Save(ct as IContentType);
        }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public override void Save(Type itemType)
        {
            var contentTypes = Service.GetAllContentTypes();
            var attr = itemType.GetCustomAttribute<DocTypeAttribute>();
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

            Save(ct);
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
                var currentTemplate = ApplicationContext.Current.Services.FileService.GetTemplate(templateAlias);
                templates.Add(currentTemplate ?? CreateTemplate(itemType, templateAlias));
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
        private static ITemplate CreateTemplate(Type itemType, string templateAlias)
        {
            var renderingEngine = UmbracoConfig.For.UmbracoSettings().Templates.DefaultRenderingEngine;
            var templateLocation = string.Empty;
            var template = new Template(templateAlias, templateAlias);

            if (renderingEngine == RenderingEngine.Mvc)
            {
                template.Content = "@inherits UmbracoTemplatePage<" + itemType.FullName + ">\n\n<h1>@Model.Content.Name</h1>";
                templateLocation = HostingEnvironment.MapPath(string.Format("~/Views/{0}.cshtml", templateAlias));
            }
            else if (renderingEngine == RenderingEngine.WebForms)
            {
                template.Content = "<%@ Master Language=\"C#\" AutoEventWireup=\"true\" %>\n\n<h1><umbraco:item field=\"pageName\" runat=\"server\" /></h1>";
                templateLocation = HostingEnvironment.MapPath(string.Format("~/masterpages/{0}.master", templateAlias));
            }

            // Try read existing content
            if (!string.IsNullOrEmpty(templateLocation))
            {
                if (System.IO.File.Exists(templateLocation))
                {
                    template.Content = System.IO.File.ReadAllText(templateLocation);
                }
            }

            ApplicationContext.Current.Services.FileService.SaveTemplate(template);
            return ApplicationContext.Current.Services.FileService.GetTemplate(templateAlias);
        }
    }
}