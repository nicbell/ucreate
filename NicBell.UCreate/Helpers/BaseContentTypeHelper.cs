using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Helpers
{
    public abstract class BaseContentTypeHelper<T> : BaseTypeHelper<T> where T : BaseContentTypeAttribute
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

        public abstract void Save(Type itemType);

        public abstract void SaveAllowedTypes(Type itemType);


        /// <summary>
        /// Sync all items
        /// </summary>
        public override void SyncAll()
        {
            var firstLevelTypes = TypesToSync.Where(x => x.BaseType == null || x.BaseType == typeof(Object));

            foreach (var itemType in firstLevelTypes)
            {
                SyncItem(itemType, true); //Deep sync
            }

            foreach (var itemType in TypesToSync)
            {
                SaveAllowedTypes(itemType);
            }
        }


        /// <summary>
        /// Sync single item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="syncChildren"></param>
        public void SyncItem(Type itemType, bool syncChildren = false)
        {
            Save(itemType);

            if (syncChildren)
            {
                var childTypes = TypesToSync.Where(x => x.BaseType == itemType);

                foreach (var childType in childTypes)
                {
                    SyncItem(childType, syncChildren);
                }
            }
        }


        /// <summary>
        /// Maps properties
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="itemType"></param>
        /// <param name="overwrite"></param>
        protected void MapProperties(IContentTypeBase ct, Type itemType)
        {
            foreach (PropertyInfo propInfo in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyAttribute propAttr = Attribute.GetCustomAttribute(propInfo, typeof(PropertyAttribute), false) as PropertyAttribute;

                if (propAttr != null)
                {
                    ct.RemovePropertyType(propAttr.Alias);

                    if (!String.IsNullOrEmpty(propAttr.TabName))
                    {
                        ct.AddPropertyGroup(propAttr.TabName);
                        ct.AddPropertyType(propAttr.GetPropertyType(), propAttr.TabName);
                    }
                    else
                    {
                        ct.AddPropertyType(propAttr.GetPropertyType());
                    }
                }
            }
        }


        /// <summary>
        /// Maps allowed children
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="allowedTypeAliases"></param>
        protected void MapAllowedTypes(IContentTypeBase ct, string[] allowedTypeAliases)
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

            ct.AllowedContentTypes = allowedTypes;
        }


        /// <summary>
        /// Sets parent ID for content type inheritance
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="ct"></param>
        protected void SetParent(IContentTypeComposition ct, Type itemType)
        {
            var parentAttr = Attribute.GetCustomAttributes(itemType.BaseType).FirstOrDefault(x => x is BaseContentTypeAttribute) as BaseContentTypeAttribute;

            if (parentAttr != null)
            {
                ct.SetLazyParentId(new Lazy<int>(() => GetByAlias(parentAttr.Alias).Id));
                ct.AddContentType(GetByAlias(parentAttr.Alias));
            }
        }
    }
}
