using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Helpers
{
    public abstract class BaseContentTypeHelper
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


        public abstract IContentTypeComposition GetByAlias(string alias);


        protected void MapProperties(IContentTypeBase mt, Type itemType, bool overwrite)
        {
            foreach (PropertyInfo propInfo in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyAttribute propAttr = Attribute.GetCustomAttribute(propInfo, typeof(PropertyAttribute), false) as PropertyAttribute;

                if (propAttr != null)
                {
                    if (overwrite)
                        mt.RemovePropertyType(propAttr.Alias);

                    if (!String.IsNullOrEmpty(propAttr.TabName))
                    {
                        mt.AddPropertyGroup(propAttr.TabName);
                        mt.AddPropertyType(propAttr.GetPropertyType(), propAttr.TabName);
                    }
                    else
                    {
                        mt.AddPropertyType(propAttr.GetPropertyType());
                    }
                }
            }
        }


        protected void MapAllowedTypes(IContentTypeBase mt, string[] allowedTypeAliases)
        {
            if (allowedTypeAliases == null || allowedTypeAliases.Length == 0)
                return;

            var allowedTypes = new List<ContentTypeSort>();

            foreach (var allowedTypeAlias in allowedTypeAliases)
            {
                allowedTypes.Add(new ContentTypeSort
                {
                    Id = new Lazy<int>(() => GetByAlias(allowedTypeAlias).Id),
                    Alias = allowedTypeAlias
                });
            }

            mt.AllowedContentTypes = allowedTypes;
        }
    }
}
