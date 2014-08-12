using NicBell.UCreate.Helpers;
using NicBell.UCreate.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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
                if (ConfigurationManager.AppSettings["umbracoConfigurationStatus"] != null && String.IsNullOrEmpty(ConfigurationManager.AppSettings["umbracoConfigurationStatus"]))
                    return;

                if (ConfigurationManager.AppSettings["UCreateDisabled"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["UCreateDisabled"]))
                    return;

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
            var dataSync = new DataTypeHelper();
            var mediaSync = new MediaTypeHelper();
            var docSync = new DocTypeHelper();
            
            //Sync all the types
            dataSync.SyncAll();
            mediaSync.SyncAll();
            docSync.SyncAll();

            //Syncing tasks that user wants to run.
            var syncTaskTypes = GetTypesThatImplementInterface(typeof(ISyncTask));

            foreach (var syncTaskType in syncTaskTypes)
            {
                var task = Activator.CreateInstance(syncTaskType) as ISyncTask;
                task.Run();
            }
        }


        /// <summary>
        /// Gets all types that implement an interface
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesThatImplementInterface(Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass)
                .ToList();
        }
    }
}
