using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NicBell.UCreate.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets all types that implement an interface
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesThatImplementInterface(Type interfaceType)
        {
            return GetDependentAssemblies(Assembly.GetExecutingAssembly())
                .SelectMany(x => x.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass)
                .ToList();
        }


        /// <summary>
        /// Gets all types with an attribute
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesWithAttribute(Type attributeType)
        {
            return GetDependentAssemblies(Assembly.GetExecutingAssembly())
                 .SelectMany(x => x.GetTypes())
                 .Where(t => Attribute.IsDefined(t, attributeType))
                 .ToList();
        }


        /// <summary>
        /// Get all assemblies that reference an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetDependentAssemblies(Assembly assembly)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.ImageRuntimeVersion != "0.0.0.0")
                .Where(a => a.GetReferencedAssemblies().Select(aRef => aRef.FullName).Contains(assembly.FullName));
        }
    }
}
