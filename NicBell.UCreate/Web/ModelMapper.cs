using NicBell.UCreate.Attributes;
using System;
using System.Reflection;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Web
{
    /// <summary>
    /// Maps published content to model.
    /// </summary>
    public static class ModelMapper
    {
        /// <summary>
        /// Map IPublishedContent to model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T Map<T>(this IPublishedContent content) where T : class, new()
        {
            T model = new T();

            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyAttribute propAttr = Attribute.GetCustomAttribute(property, typeof(PropertyAttribute), false) as PropertyAttribute;

                if (propAttr != null)
                {
                    property.SetValue(model, Convert.ChangeType(content.GetProperty(propAttr.Alias).Value, property.PropertyType));
                }
            }

            return model;
        }
    }
}
