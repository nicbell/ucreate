using System;
using System.Collections.Generic;
using System.Linq;
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


        public void Save(IMediaType mt, IList<KeyValuePair<string, PropertyType>> properties, bool overwrite = false)
        {
            var mediaTypes = Service.GetAllMediaTypes();

            if (!mediaTypes.Select(x => x.Alias).Contains(mt.Alias))
            {
                MapAllowedTypesIds(mt);
                MapProperties(mt, properties);
                Service.Save(mt);
            }
            else if (overwrite)
            {
                //Item already exists do we fetch it and update it
                var existingMediaType = mediaTypes.First(x => x.Key == mt.Key);

                MapAllowedTypesIds(mt);

                existingMediaType.Name = mt.Name;
                existingMediaType.Alias = mt.Alias;
                existingMediaType.Icon = mt.Icon;
                existingMediaType.AllowedAsRoot = mt.AllowedAsRoot;
                existingMediaType.IsContainer = mt.IsContainer;
                existingMediaType.AllowedContentTypes = mt.AllowedContentTypes;

                MapProperties(existingMediaType, properties);

                Service.Save(existingMediaType);
            }
        }


        public void Save(IMediaType mt, bool overwrite = false)
        {
            var mediaTypes = Service.GetAllMediaTypes();

            if (!mediaTypes.Select(x => x.Alias).Contains(mt.Alias))
            {
                MapAllowedTypesIds(mt);
                Service.Save(mt);
            }
            else if (overwrite)
            {
                //Item already exists do we fetch it and update it
                var existingMediaType = mediaTypes.First(x => x.Key == mt.Key);

                MapAllowedTypesIds(mt);

                existingMediaType.Name = mt.Name;
                existingMediaType.Alias = mt.Alias;
                existingMediaType.Icon = mt.Icon;
                existingMediaType.AllowedAsRoot = mt.AllowedAsRoot;
                existingMediaType.IsContainer = mt.IsContainer;
                existingMediaType.AllowedContentTypes = mt.AllowedContentTypes;

                Service.Save(existingMediaType);
            }
        }


        public IMediaType GetMediaType(string alias)
        {
            return Service.GetMediaType(alias);
        }


        private void MapAllowedTypesIds(IMediaType mt)
        {
            if (mt.AllowedContentTypes != null && mt.AllowedContentTypes.Count() > 0)
            {
                foreach (var act in mt.AllowedContentTypes)
                {
                    act.Id = new Lazy<int>(() => Service.GetMediaType(act.Alias).Id);
                }
            }
        }


        private void MapProperties(IMediaType mt, IList<KeyValuePair<string, PropertyType>> properties)
        {
            var tabNames = properties.Select(x => x.Key).Distinct();

            foreach (var tabName in tabNames)
            {
                mt.AddPropertyGroup(tabName);
            }

            foreach (var property in properties)
            {
                mt.AddPropertyType(property.Value, property.Key);
            }
        }
    }
}
