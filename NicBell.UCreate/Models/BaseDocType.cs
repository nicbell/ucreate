using Newtonsoft.Json;
using NicBell.UCreate.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
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

                    //Convert custom type
                    if (propAttr.TypeConverter != null)
                    {
                        var converter = Activator.CreateInstance(propAttr.TypeConverter, null);

                        if (converter is ITypeConverter)
                        {
                            property.SetValue(this, (converter as ITypeConverter).Convert(value));
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
                                property.SetValue(this, JsonConvert.DeserializeObject<List<RelatedLink>>(value.ToString()));
                                break;
                            case Constants.PropertyTypes.Richtexteditor:
                            case Constants.PropertyTypes.Textboxmultiple:
                            case Constants.PropertyTypes.Textstring:
                                property.SetValue(this, value.ToString());
                                break;
                            default:
                                property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
                                break;
                        }
                    }
                }
            }
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
