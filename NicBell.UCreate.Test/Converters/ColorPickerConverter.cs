using System;
using System.Drawing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace NicBell.UCreate.Test.Converters
{
    [PropertyValueType(typeof(string))]
    public class ColorPickerConverter : IPropertyValueConverter
    {
        public bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals(Umbraco.Core.Constants.PropertyEditors.ColorPickerAlias);
        }

        public object ConvertDataToSource(PublishedPropertyType propertyType, object data, bool preview)
        {
            return (data ?? string.Empty).ToString();
        }

        public object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
          return  new ColorConverter().ConvertFromString("#" + (source ?? string.Empty).ToString());
        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
        {
            return (source ?? string.Empty).ToString();
        }
    }
}