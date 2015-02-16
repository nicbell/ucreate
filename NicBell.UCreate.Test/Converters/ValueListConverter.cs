using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NicBell.UCreate.Test.Converters
{
    public class ValueListConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                value = String.Empty;

            if (value is string)
            {
                var list = new List<string>();

                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    list.AddRange(value.ToString().Trim().Split(',').ToList());
                }

                return list;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}