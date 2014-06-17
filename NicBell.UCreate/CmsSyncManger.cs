using NicBell.UCreate.Attributes;
using NicBell.UCreate.Helpers;
using NicBell.UCreate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Models;

namespace NicBell.UCreate
{
    internal class CmsSyncManger
    {
        private static string _syncObj = "sync";
        private static bool _synchronized = false;

        /// <summary>
        /// Add stuff if it isn't added
        /// </summary>
        public static void SynchronizeIfNotSynchronized()
        {
            // avoid immediate locking because it will impact performance
            if (!_synchronized)
            {
                lock (_syncObj)
                {
                    if (!_synchronized)
                    {
                        Synchronize();
                        _synchronized = true;
                    }
                }
            }
        }


        /// <summary>
        /// This looks lame but it means we don't have to go into Umbraco and configure stuff.
        /// </summary>
        public static void Synchronize()
        {
            var items = GetAllItemsToSync();

            var dtSync = new DataTypeHelper();
            var mtSync = new MediaTypeHelper();


            foreach (var item in items)
            {
                //Data type
                if (item.IsSubclassOf(typeof(CustomDataTypeBase)))
                {
                    var attr = Attribute.GetCustomAttributes(item).FirstOrDefault(x => x is CustomDataTypeAttribute) as CustomDataTypeAttribute;
                    var def = new DataTypeDefinition(-1, attr.EditorAlias)
                    {
                        Name = attr.Name,
                        Key = new Guid(attr.Key),
                        DatabaseType = attr.DBType
                    };
                    var instance = Activator.CreateInstance(item, null) as CustomDataTypeBase;

                    instance.PreAdd();
                    dtSync.Save(def, instance.PreValues, attr.Overwrite);
                    //instance.Postadd();
                }

                //Media type
                if (item.IsSubclassOf(typeof(CustomMediaTypeBase)))
                {
                    var attr = Attribute.GetCustomAttributes(item).FirstOrDefault(x => x is CustomMediaTypeAttribute) as CustomMediaTypeAttribute;
                    var mt = new MediaType(-1)
                    {
                        Key = new Guid(attr.Key),
                        Name = attr.Name,
                        Alias = attr.Alias,
                        Icon = attr.Icon,
                        AllowedAsRoot = attr.AllowedAsRoot,
                        IsContainer = attr.IsContainer
                    };
                    var instance = Activator.CreateInstance(item, null) as CustomMediaTypeBase;

                    instance.PreAdd();
                    mtSync.Save(mt, instance.Properties, attr.Overwrite);
                    instance.PostAdd();
                }
            }
        }


        public static IEnumerable<Type> GetAllItemsToSync()
        {
            var allTypes = Assembly.GetCallingAssembly().GetTypes();

            var items = new List<KeyValuePair<int, Type>>();

            foreach (Type t in allTypes)
            {
                var attrs = Attribute.GetCustomAttributes(t);

                // Displaying output. 
                foreach (Attribute attr in attrs)
                {
                    if (attr is OrderdSyncAttribute)
                    {
                        items.Add(new KeyValuePair<int, Type>(((OrderdSyncAttribute)attr).SyncOrder, t));
                    }
                }
            }

            return items.OrderBy(x => x.Key).Select(x => x.Value);
        }
    }
}
