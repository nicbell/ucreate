using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using System;
using System.Linq;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Helpers
{
    public class MediaTypeHelper : BaseContentTypeHelper
    {
        public MediaTypeHelper() { }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public void Save(Type itemType)
        {  
            var mediaTypes = Service.GetAllMediaTypes();
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is MediaTypeAttribute) as MediaTypeAttribute;

            if (!mediaTypes.Any(x => x.Key == new Guid(attr.Key)) || attr.Overwrite)
            {
                var instance = Activator.CreateInstance(itemType, null);
                var mt = mediaTypes.FirstOrDefault(x => x.Key == new Guid(attr.Key)) ?? new MediaType(-1) { Key = new Guid(attr.Key) };

                mt.Name = attr.Name;
                mt.Alias = attr.Alias;
                mt.Icon = attr.Icon;
                mt.AllowedAsRoot = attr.AllowedAsRoot;
                mt.IsContainer = attr.IsContainer;

                SetParent(mt, itemType);
                MapAllowedTypes(mt, attr.AllowedTypes);
                MapProperties(mt, itemType, attr.Overwrite);

                if (instance is IHasPrePostHooks)
                    ((IHasPrePostHooks)instance).PreAdd();

                Service.Save(mt);

                if (instance is IHasPrePostHooks)
                    ((IHasPrePostHooks)instance).PostAdd();
            }
        }


        /// <summary>
        /// Sets parent ID
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="ct"></param>
        private void SetParent(IContentTypeComposition ct, Type itemType)
        {
            var parentAttr = Attribute.GetCustomAttributes(itemType.BaseType).FirstOrDefault(x => x is MediaTypeAttribute) as MediaTypeAttribute;

            if (parentAttr != null)
            {
                ct.SetLazyParentId(new Lazy<int>(() => GetByAlias(parentAttr.Alias).Id));
                ct.AddContentType(GetByAlias(parentAttr.Alias));
            }
        }


        public override IContentTypeComposition GetByAlias(string alias)
        {
            return Service.GetMediaType(alias);
        }
    }
}
