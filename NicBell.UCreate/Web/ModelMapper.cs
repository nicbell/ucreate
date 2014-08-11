using Newtonsoft.Json;
using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
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
                    var value = content.GetProperty(propAttr.Alias).Value;

                    //Convert custom type
                    if (propAttr.TypeConverter != null)
                    {
                        var converter = Activator.CreateInstance(propAttr.TypeConverter, null);

                        if (converter is ITypeConverter)
                        {
                            property.SetValue(model, (converter as ITypeConverter).Convert(value));
                        }
                        else
                        {
                            throw new Exception("Converter must implement: ITypeConverter");
                        }
                    }
                    else
                    {
                        switch (propAttr.TypeName)
                        {
                            case Constants.PropertyTypes.RelatedLinks:
                                property.SetValue(model, JsonConvert.DeserializeObject<List<RelatedLink>>(value.ToString()));
                                break;
                            case Constants.PropertyTypes.Richtexteditor:
                            case Constants.PropertyTypes.Textboxmultiple:
                            case Constants.PropertyTypes.Textstring:
                            default:
                                property.SetValue(model, Convert.ChangeType(value, property.PropertyType));
                                break;
                        }
                    }
                }
            }

            return model;
        }
    }


    /// <summary>
    /// Interface for converters
    /// </summary>
    public interface ITypeConverter
    {
        object Convert(object value);
    }
}
