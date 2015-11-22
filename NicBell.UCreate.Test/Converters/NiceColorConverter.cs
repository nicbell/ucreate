using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace NicBell.UCreate.Test.Converters
{
    public class NiceColorConverter : ColorConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(int))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return Color.Empty;

            if (value is string && string.IsNullOrEmpty(value.ToString()))
                return Color.Empty;

            // Standard ColorConverter needs #
            return base.ConvertFrom(context, culture, "#" + value.ToString());
        }
    }
}