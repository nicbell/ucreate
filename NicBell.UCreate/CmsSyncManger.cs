using Newtonsoft.Json;
using NicBell.UCreate.Helpers;
using NicBell.UCreate.Interfaces;
using NicBell.UCreate.Sync;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NicBell.UCreate
{
    internal class CmsSyncManger
    {
        private static string _syncObj = "sync";
        private static bool _synchronized = false;
        private static string _syncVersionFile = "ucreate.json";

        /// <summary>
        /// Add stuff if it isn't added
        /// </summary>
        public static void SynchronizeIfNotSynchronized()
        {
            // avoid immediate locking because it will impact performance
            if (!_synchronized)
            {
                if (!SyncRequired())
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

            UpDateSyncVersion();
        }


        /// <summary>
        /// Updates sync version JSON
        /// </summary>
        public static void UpDateSyncVersion()
        {
            var assemblies = ReflectionHelper.GetDependentAssemblies(Assembly.GetExecutingAssembly())
                .Select(x => new AssemblyVersion
                {
                    Name = x.GetName().FullName,
                    Version = x.GetName().Version.ToString(),
                    Modified = new FileInfo(x.Location).LastWriteTimeUtc
                });

            var fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/app_data/"), _syncVersionFile);
            var json = JsonConvert.SerializeObject(assemblies.ToArray());

            using (var myFile = File.Open(fileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(myFile))
                {
                    writer.Write(json);
                }
            }
        }


        /// <summary>
        /// Check if sync is required
        /// </summary>
        /// <returns></returns>
        public static bool SyncRequired()
        {
            if (ConfigurationManager.AppSettings["umbracoConfigurationStatus"] != null && String.IsNullOrEmpty(ConfigurationManager.AppSettings["umbracoConfigurationStatus"]))
                return false;

            if (ConfigurationManager.AppSettings["UCreateDisabled"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["UCreateDisabled"]))
                return false;


            using (var stream = File.OpenText(filename))
            {
                using (var reader = new JsonTextReader(stream))
                {
                    var serializer = new JsonSerializer();
                    var entries = serializer.Deserialize<AssemblyVersion[]>(reader);
                }
            }

            return true;
        }
    }


    public class AssemblyVersion
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime Modified { get; set; }
    }
}
