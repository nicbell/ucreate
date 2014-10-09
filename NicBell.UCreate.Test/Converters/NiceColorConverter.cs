using NicBell.UCreate;
using NicBell.UCreate.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace NicBell.UCreate.Test.Converters
{
    public class NiceColorConverter : ITypeConverter
    {
        public object Convert(object value)
        {
            if (!String.IsNullOrEmpty(value.ToString()))
                return ColorTranslator.FromHtml("#" + value.ToString());
            return Color.Empty;
        }
    }
}