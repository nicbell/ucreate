using NicBell.UCreate.Attributes;
using System;
using System.ComponentModel;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace NicBell.UCreate.Models
{
    public abstract class BaseDocType : PublishedContentModel
    {
        public BaseDocType(IPublishedContent content)
            : base(content)
        {
            SetValues(content);
        }


        /// <summary>
        /// This is magic
        /// </summary>
        /// <param name="content"></param>
        private void SetValues(IPublishedContent content)
        {
            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyAttribute propAttr = Attribute.GetCustomAttribute(property, typeof(PropertyAttribute), false) as PropertyAttribute;

                if (propAttr != null)
                {
                    var value = content.GetProperty(propAttr.Alias).Value;

                    //Custom converter..
                    var converterAttr = property.GetCustomAttribute<TypeConverterAttribute>();

                    if (converterAttr != null)
                    {
                        var converter = Activator.CreateInstance(Type.GetType(converterAttr.ConverterTypeName)) as TypeConverter;
                        property.SetValue(this, converter.ConvertFrom(value ?? String.Empty));
                    }
                    else
                    {
                        var convert = value.TryConvertTo(property.PropertyType);
                        if (convert.Success)
                        {
                            property.SetValue(this, convert.Result);
                        }
                    }
                }
            }
        }
    }
}
