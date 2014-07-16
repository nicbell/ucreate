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
    public class MediaTypeHelper
    {
        public MediaTypeHelper() { }


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


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public void Save(Type itemType)
        {  
            var mediaTypes = Service.GetAllMediaTypes();
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is CustomMediaTypeAttribute) as CustomMediaTypeAttribute;

            if (!mediaTypes.Any(x => x.Key == new Guid(attr.Key)) || attr.Overwrite)
            {
                var mt = mediaTypes.FirstOrDefault(x => x.Key == new Guid(attr.Key)) ?? new MediaType(-1) { Key = new Guid(attr.Key) };

                mt.Name = attr.Name;
                mt.Alias = attr.Alias;
                mt.Icon = attr.Icon;
                mt.AllowedAsRoot = attr.AllowedAsRoot;
                mt.IsContainer = attr.IsContainer;

                MapAllowedTypes(mt, attr.AllowedTypes);
                MapProperties(mt, itemType, attr.Overwrite);

                Service.Save(mt);
            }
        }


        public IMediaType GetMediaType(string alias)
        {
            return Service.GetMediaType(alias);
        }


        private void MapAllowedTypes(IContentTypeBase mt, string[] allowedTypeAliases)
        {
            var allowedTypes = new List<ContentTypeSort>();

            foreach (var allowedTypeAlias in allowedTypeAliases)
            {
                allowedTypes.Add(new ContentTypeSort
                {
                    Id = new Lazy<int>(() => Service.GetMediaType(allowedTypeAlias).Id),
                    Alias = allowedTypeAlias
                });
            }

            mt.AllowedContentTypes = allowedTypes;
        }


        private void MapProperties(IMediaType mt, Type itemType, bool overwrite)
        {
            foreach (PropertyInfo propInfo in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                CustomTypePropertyAttribute propAttr = Attribute.GetCustomAttribute(propInfo, typeof(CustomTypePropertyAttribute), false) as CustomTypePropertyAttribute;

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
    }
}
