using NicBell.UCreate.Helpers;
using NicBell.UCreate.Interfaces;
using NicBell.UCreate.Sync;
using System;
using System.Configuration;

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
                    if (ConfigurationManager.AppSettings["umbracoConfigurationStatus"] != null && String.IsNullOrEmpty(ConfigurationManager.AppSettings["umbracoConfigurationStatus"]))
                        return;

                    if (ConfigurationManager.AppSettings["UCreateEnabled"] == null || !Convert.ToBoolean(ConfigurationManager.AppSettings["UCreateEnabled"]))
                        return;

                    if (!_synchronized)
                    {
                        if (!AssemblyVersionHelper.IsSynced())
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
            var dataSync = new DataTypeSync();
            var mediaSync = new MediaTypeSync();
            var docSync = new DocTypeSync();

            //Sync all the types
            dataSync.SyncAll();
            mediaSync.SyncAll();
            docSync.SyncAll();

            //Syncing tasks that user wants to run.
            var syncTaskTypes = ReflectionHelper.GetTypesThatImplementInterface(typeof(ISyncTask));

            foreach (var syncTaskType in syncTaskTypes)
            {
                var task = Activator.CreateInstance(syncTaskType) as ISyncTask;
                task.Run();
            }

            AssemblyVersionHelper.Update();
        }
    }
}
