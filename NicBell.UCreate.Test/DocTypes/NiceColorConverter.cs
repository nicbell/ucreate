using NicBell.UCreate;
using NicBell.UCreate.Models;
using System;
using System.Drawing;

namespace NicBell.UCreate.Test.DocTypes
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
