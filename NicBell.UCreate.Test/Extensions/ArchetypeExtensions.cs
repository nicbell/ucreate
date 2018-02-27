using Archetype.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NicBell.UCreate.Test.Extensions
{
    public static class ArchetypeExtensions
    {
        public static T As<T>(this ArchetypeFieldsetModel fieldset)
        {
            var instance = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => !prop.IsDefined(typeof(JsonIgnoreAttribute)));


            foreach (var propInfo in properties)
            {
                var jsonPropAttribute = propInfo.GetCustomAttribute<JsonPropertyAttribute>(true);

                var propertyAlias = jsonPropAttribute != null ? jsonPropAttribute.PropertyName : propInfo.Name;
                var propertyType = propInfo.PropertyType;

                if (!fieldset.HasProperty(propertyAlias) && !fieldset.HasValue(propertyAlias))
                    continue;

                propInfo.SetValue(instance, fieldset.GetType().GetMethods().First(m => m.IsGenericMethod && m.Name.Equals("GetValue"))
                    .MakeGenericMethod(propertyType)
                    .Invoke(fieldset, new object[] { propertyAlias }));
            }

            return instance;
        }

        public static List<T> ToList<T>(this ArchetypeModel model) {
            var items = new List<T>();

            if (model is ArchetypeModel)
            {
                foreach (var fieldset in (model as ArchetypeModel))
                {
                    items.Add(fieldset.As<T>());
                }
            }

            return items;   
        }
    }
}