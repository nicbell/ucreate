using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NicBell.UCreate.Helpers
{
    public static class AssemblyVersionHelper
    {
        private const string SyncVersionFile = "ucreate.json";

        /// <summary>
        /// Updates sync version JSON
        /// </summary>
        public static void Update()
        {
            var assemblyVersions = GetAssemblyVersions();
            var fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/app_data/"), SyncVersionFile);

            using (var myFile = File.Open(fileName, FileMode.Create))
            using (var writer = new StreamWriter(myFile))
            {
                var json = JsonConvert.SerializeObject(assemblyVersions);
                writer.Write(json);
            }
        }


        /// <summary>
        /// Checks if current assemblies have been synced
        /// </summary>
        /// <returns></returns>
        public static bool IsSynced()
        {
            var assemblyVersions = GetAssemblyVersions(); 
            var fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/app_data/"), SyncVersionFile);

            if (File.Exists(fileName))
            {
                using (var stream = File.OpenText(fileName))
                using (var reader = new JsonTextReader(stream))
                {
                    var entries = new JsonSerializer().Deserialize<List<AssemblyVersion>>(reader);
                    return assemblyVersions.SequenceEqual(entries);
                }
            }

            return false;
        }


        /// <summary>
        /// Gets versions from .dlls
        /// </summary>
        /// <returns></returns>
        private static List<AssemblyVersion> GetAssemblyVersions()
        {
            return ReflectionHelper.GetDependentAssemblies(Assembly.GetExecutingAssembly())
                .Select(x => new AssemblyVersion
                {
                    FullName = x.GetName().FullName,
                    Modified = new FileInfo(x.Location).LastWriteTimeUtc
                })
                .ToList();
        }
    }


    public struct AssemblyVersion
    {
        public string FullName { get; set; }
        public DateTime Modified { get; set; }
    }
}
