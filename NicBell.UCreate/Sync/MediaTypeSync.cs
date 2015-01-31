using NicBell.UCreate.Attributes;
using System;
using System.Linq;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Sync
{
    public class MediaTypeSync : BaseTreeContentTypeSync<MediaTypeAttribute>
    {
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
            var mt = mediaTypes.FirstOrDefault(x => x.Alias == itemType.Name) ?? new MediaType(-1);

            mt.Name = attr.Name;
            mt.Alias = itemType.Name;
            mt.Icon = attr.Icon;
            mt.AllowedAsRoot = attr.AllowedAsRoot;
            mt.IsContainer = attr.IsContainer;

            SetParent(mt, itemType);
            MapProperties(mt, itemType);

            Service.Save(mt);
        }
    }
}
