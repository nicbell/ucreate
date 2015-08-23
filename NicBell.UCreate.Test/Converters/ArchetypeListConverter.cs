using Archetype.Models;
using NicBell.UCreate.Test.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace NicBell.UCreate.Test.Converters
{
    public class ArchetypeListConverter<T> : TypeConverter where T : class
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(ArchetypeModel))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var items = new List<T>();

            if (value is ArchetypeModel)
            {
                foreach (var fieldset in (value as ArchetypeModel))
                {
                    items.Add(fieldset.As<T>());
                }
            }

            return items;
        }
    }
}