using Archetype.Models;
using Newtonsoft.Json;
using System;
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
                .Where(prop => !Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)));


            foreach (var propInfo in properties)
            {
                var attributes = propInfo.GetCustomAttributes(true);
                var jsonPropAttribute = (JsonPropertyAttribute)attributes.FirstOrDefault(attr => attr is JsonPropertyAttribute);

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
    }
}