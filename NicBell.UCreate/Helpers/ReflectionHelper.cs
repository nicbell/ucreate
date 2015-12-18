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
        public static List<Type> GetTypesThatImplementInterface<T>()
        {
            return GetDependentAssemblies(Assembly.GetExecutingAssembly())
                .SelectMany(x => x.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && t.IsClass)
                .ToList();
        }


        /// <summary>
        /// Gets all types with an attribute
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static List<Type> GetTypesWithAttribute<T>() where T : Attribute
        {
            return GetDependentAssemblies(Assembly.GetExecutingAssembly())
                 .SelectMany(x => x.GetTypes())
                 .Where(t => t.IsDefined(typeof(T)))
                 .ToList();
        }


        /// <summary>
        /// Gets all properties of a type with an attribute
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertiesWithAttribute<T>(Type itemType, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, bool inherit = false) where T : Attribute
        {
            return itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.IsDefined(typeof(T), inherit))
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
