using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NicBell.UCreate.Test.Converters
{
    public class IdListConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string || value is int)
            {
                var list = new List<int>();

                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    list.AddRange(value.ToString().Trim().Split(',').Select(int.Parse).ToList());
                }

                return list;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}