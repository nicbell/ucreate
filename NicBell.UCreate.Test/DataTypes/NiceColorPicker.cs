using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using NicBell.UCreate.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DataTypes
{
    [DataType(EditorAlias = Umbraco.Core.Constants.PropertyEditors.ColorPickerAlias,
        Name = "Nice Color Picker",
        Key = "1bfca1e7-95d0-485e-bd94-9fe9c2b8821f",
        DBType = DataTypeDatabaseType.Nvarchar)]
    public class NiceColorPicker : IHasPreValues
    {

        public IDictionary<string, PreValue> PreValues
        {
            get
            {
                return new Dictionary<string, PreValue> {
                    {"1", new PreValue("ff00ff")},
                    {"2", new PreValue("1f00f1")},
                    {"3", new PreValue("123123")},
                    {"4", new PreValue("ffffff")}
                };
            }
        }
    }
}