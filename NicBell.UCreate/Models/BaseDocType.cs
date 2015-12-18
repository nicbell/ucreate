using NicBell.UCreate.Attributes;
using NicBell.UCreate.Helpers;
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
        /// Gets all properties with PropertyAttribute and sets their value
        /// </summary>
        /// <param name="content"></param>
        private void SetValues(IPublishedContent content)
        {
            var properties = ReflectionHelper.GetPropertiesWithAttribute<PropertyAttribute>(GetType(), bindingFlags: BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var alias = property.GetCustomAttribute<PropertyAttribute>(false).Alias;
                var publishedProperty = content.GetProperty(alias);

                //Local links cause stack overflow when accessing value during publish.
                //Use @Html.Raw(TemplateUtilities.ParseInternalLinks(value)) to resolve these URLs on the template.
                if (publishedProperty.DataValue is string && (publishedProperty.DataValue as string).Contains("localLink"))
                {
                    SetValue(property, publishedProperty.DataValue);
                }
                else
                {
                    SetValue(property, publishedProperty.Value);
                }
            }
        }


        /// <summary>
        /// Set value for a property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void SetValue(PropertyInfo property, object value)
        {
            //Check for type converter..
            if (property.IsDefined(typeof(TypeConverterAttribute)))
            {
                var converterAttr = property.GetCustomAttribute<TypeConverterAttribute>();
                var converter = Activator.CreateInstance(Type.GetType(converterAttr.ConverterTypeName)) as TypeConverter;
                property.SetValue(this, converter.ConvertFrom(value ?? string.Empty));
            }
            else
            {
                var convert = value.TryConvertTo(property.PropertyType);
                if (convert.Success)
                    property.SetValue(this, convert.Result);
            }
        }
    }
}
