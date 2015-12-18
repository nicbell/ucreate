using NicBell.UCreate.Attributes;
using System;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Sync
{
    public class MemberGroupSync : BaseTypeSync<MemberGroupAttribute>
    {
        /// <summary>
        /// Service
        /// </summary>
        public IMemberGroupService Service
        {
            get
            {
                return ApplicationContext.Current.Services.MemberGroupService;
            }
        }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public override void Save(Type itemType)
        {
            var memberGroups = Service.GetAll();
            var attr = itemType.GetCustomAttribute<MemberGroupAttribute>();
            var mt = memberGroups.FirstOrDefault(x => x.Name == itemType.Name) ?? new MemberGroup();

            mt.Name = attr.Name;

            Service.Save(mt);
        }
    }
}
