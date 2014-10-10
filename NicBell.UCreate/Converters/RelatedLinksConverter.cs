using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace NicBell.UCreate.Converters
{
    public class RelatedLinksConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(JArray) || sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is JArray || value is string)
                return JsonConvert.DeserializeObject<List<RelatedLink>>(value.ToString());

            return base.ConvertFrom(context, culture, value);
        }
    }
}
