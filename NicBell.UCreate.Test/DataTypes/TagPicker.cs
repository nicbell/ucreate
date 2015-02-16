using System.Collections.Generic;
using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DataTypes
{
    [DataType(EditorAlias = Umbraco.Core.Constants.PropertyEditors.CheckBoxListAlias,
       Name = "Tag Picker",
       Key = "75e68783-5fbe-4346-97a9-60b65fb2fb24",
       DBType = DataTypeDatabaseType.Nvarchar)]
    public class TagPicker : IHasPreValues
    {
        public IDictionary<string, PreValue> PreValues
        {
            get
            {
                return new Dictionary<string, PreValue> {
                    {"1", new PreValue("Tag1")},
                    {"2", new PreValue("Tag2 New Name")},
                    {"3", new PreValue("Tag3")},
                    {"4", new PreValue("Tag4")}
                };
            }
        }
    }
}