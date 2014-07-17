using NicBell.UCreate.Attributes;
using NicBell.UCreate.Helpers;
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

            var docSync = new DocTypeHelper();
            var dataSync = new DataTypeHelper();
            var mediaSync = new MediaTypeHelper();

            foreach (var item in items)
            {
                //Document type
                if (item.GetCustomAttribute<DocTypeAttribute>() != null)
                {
                    docSync.Save(item);
                }

                //Data type
                if (item.GetCustomAttribute<DataTypeAttribute>() != null)
                {
                    dataSync.Save(item);
                }

                //Media type
                if (item.GetCustomAttribute<MediaTypeAttribute>() != null)
                {
                    mediaSync.Save(item);
                }
            }
        }


        public static IEnumerable<Type> GetAllItemsToSync()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());

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
