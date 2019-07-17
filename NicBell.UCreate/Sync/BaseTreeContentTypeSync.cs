﻿using NicBell.UCreate.Attributes;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace NicBell.UCreate.Sync
{
    public abstract class BaseTreeContentTypeSync<T> : BaseContentTypeSync<T> where T : BaseTreeContentTypeAttribute
    {
        public abstract IContentTypeComposition GetByAlias(string alias);

        /// <summary>
        /// Saves a content type, useful for updating
        /// </summary>
        /// <param name="ct"></param>
        public abstract void Save(IContentTypeBase ct);


        /// <summary>
        /// Sync all items
        /// </summary>
        public override void SyncAll()
        {
            // Get first level types to sync as a tree:
            // they could be types that don't inherit from anything,
            // inherit directly from PublishedContentModel,
            // inherit from some other POCO that is's a content type
            Func<Type, bool> isFirstLevelType = x => x.BaseType == null
                || x.BaseType == typeof(object)
                || x.BaseType == typeof(PublishedContentModel)
                || !x.BaseType.GetCustomAttributes().Any(at => at is BaseContentTypeAttribute);

            var firstLevelTypes = TypesToSync.Where(isFirstLevelType);

            foreach (var itemType in firstLevelTypes)
            {
                SyncItem(itemType, true); //Deep sync
            }

            foreach (var itemType in TypesToSync)
            {
                SaveAllowedTypes(itemType);
                SaveCompositions(itemType);
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
        /// Saves allowed types
        /// </summary>
        /// <param name="itemType"></param>
        public void SaveAllowedTypes(Type itemType)
        {
            var attr = itemType.GetCustomAttribute<T>();
            var ct = GetByAlias(itemType.Name);
            MapAllowedTypes(ct, attr.AllowedChildTypes);

            Save(ct);
        }


        /// <summary>
        /// Maps allowed children
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="allowedTypeAliases"></param>
        protected void MapAllowedTypes(IContentTypeBase ct, Type[] allowedTypes)
        {
            if (allowedTypes == null || allowedTypes.Length == 0)
                return;

            var allowedContentTypes = new List<ContentTypeSort>();

            for (int i = 0; i < allowedTypes.Length; i++)
            {
                var allowedType = allowedTypes[i];
                allowedContentTypes.Add(new ContentTypeSort(new Lazy<int>(() => GetByAlias(allowedType.Name).Id), i, allowedType.Name));
            }

            ct.AllowedContentTypes = allowedContentTypes;
        }


        /// <summary>
        /// Saves content type compositions
        /// </summary>
        /// <param name="itemType"></param>
        public void SaveCompositions(Type itemType)
        {
            var attr = itemType.GetCustomAttribute<T>();

            if (attr.CompositionTypes == null || attr.CompositionTypes.Length == 0)
                return;

            var ct = GetByAlias(itemType.Name);

            foreach (var compositionType in attr.CompositionTypes)
            {
                ct.AddContentType(GetByAlias(compositionType.Name));
            }


            Save(ct);
        }


        /// <summary>
        /// Sets parent ID for content type inheritance
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="ct"></param>
        protected void SetParent(IContentTypeComposition ct, Type itemType)
        {
            var parentAttr = itemType.BaseType.GetCustomAttributes().FirstOrDefault(x => x is BaseContentTypeAttribute) as BaseContentTypeAttribute;

            if (parentAttr != null)
            {
                ct.SetLazyParentId(new Lazy<int>(() => GetByAlias(itemType.BaseType.Name).Id));
                ct.AddContentType(GetByAlias(itemType.BaseType.Name));
            }
        }
    }
}
