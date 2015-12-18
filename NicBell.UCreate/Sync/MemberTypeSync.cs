using NicBell.UCreate.Attributes;
using NicBell.UCreate.Helpers;
using System;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Sync
{
    public class MemberTypeSync : BaseContentTypeSync<MemberTypeAttribute>
    {
        /// <summary>
        /// Service
        /// </summary>
        public IMemberTypeService Service
        {
            get
            {
                return ApplicationContext.Current.Services.MemberTypeService;
            }
        }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public override void Save(Type itemType)
        {
            var memberTypes = Service.GetAll();
            var attr = itemType.GetCustomAttribute<MemberTypeAttribute>();
            var mt = memberTypes.FirstOrDefault(x => x.Alias == itemType.Name) ?? new MemberType(-1);

            mt.Name = attr.Name;
            mt.Alias = itemType.Name;
            mt.Description = attr.Description;
            mt.Icon = attr.Icon;

            MapProperties(mt, itemType);

            //Set member specific properties
            var memberProperties = ReflectionHelper.GetPropertiesWithAttribute<MemberPropertyAttribute>(itemType)
                .Select(prop => prop.GetCustomAttribute<MemberPropertyAttribute>(false));

            foreach (var memberProperty in memberProperties)
            {
                mt.SetMemberCanEditProperty(memberProperty.Alias, memberProperty.CanEdit);
                mt.SetMemberCanViewProperty(memberProperty.Alias, memberProperty.ShowOnProfile);
            }

            Service.Save(mt);
        }
    }
}