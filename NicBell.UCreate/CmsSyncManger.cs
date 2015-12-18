using NicBell.UCreate.Helpers;
using NicBell.UCreate.Interfaces;
using NicBell.UCreate.Sync;
using System;
using System.Linq;

namespace NicBell.UCreate
{
    internal class CmsSyncManger
    {
        private static readonly object Mutex = new object();
        private static bool _synchronized;

        /// <summary>
        /// Add stuff if it isn't added
        /// </summary>
        public static void SynchronizeIfNotSynchronized()
        {
            // avoid immediate locking because it will impact performance
            if (!_synchronized)
            {
                lock (Mutex)
                {
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
            var memberSync = new MemberTypeSync();
            var memberGroupSync = new MemberGroupSync();

            //Sync all the types
            dataSync.SyncAll();
            mediaSync.SyncAll();
            docSync.SyncAll();
            memberSync.SyncAll();
            memberGroupSync.SyncAll();

            //Syncing tasks that user wants to run.
            var syncTaskTypes = ReflectionHelper.GetTypesThatImplementInterface<ISyncTask>();

            foreach (var task in syncTaskTypes.Select(syncTaskType => Activator.CreateInstance(syncTaskType) as ISyncTask))
            {
                task.Run();
            }

            AssemblyVersionHelper.Update();
        }
    }
}
