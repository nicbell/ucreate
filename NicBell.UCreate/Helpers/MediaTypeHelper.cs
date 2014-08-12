using NicBell.UCreate.Attributes;
using System;
using System.Linq;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Helpers
{
    public class MediaTypeHelper : BaseContentTypeHelper<MediaTypeAttribute>
    {
        public MediaTypeHelper() { }


        public override IContentTypeComposition GetByAlias(string alias)
        {
            return Service.GetMediaType(alias);
        }

        
        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public override void Save(Type itemType)
        {
            var mediaTypes = Service.GetAllMediaTypes();
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is MediaTypeAttribute) as MediaTypeAttribute;
            var mt = mediaTypes.FirstOrDefault(x => x.Key == new Guid(attr.Key)) ?? new MediaType(-1) { Key = new Guid(attr.Key) };

            mt.Name = attr.Name;
            mt.Alias = attr.Alias;
            mt.Icon = attr.Icon;
            mt.AllowedAsRoot = attr.AllowedAsRoot;
            mt.IsContainer = attr.IsContainer;

            SetParent(mt, itemType);
            MapProperties(mt, itemType);

            Service.Save(mt);
        }


        public override void SaveAllowedTypes(Type itemType)
        {
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is MediaTypeAttribute) as MediaTypeAttribute;
            var mt = GetByAlias(attr.Alias) as IMediaType;

            MapAllowedTypes(mt, attr.AllowedTypes);

            Service.Save(mt);
        }
    }
}
