using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DataTypes
{
    [DataType(EditorAlias = Umbraco.Core.Constants.PropertyEditors.MultiNodeTreePickerAlias,
        Name = "Blog Post Picker",
        Key = "cefd1be0-d206-4e2c-8f42-1d909260b513",
        DBType = DataTypeDatabaseType.Nvarchar)]
    public class BlogPostPicker : IHasPreValues
    {
        public IDictionary<string, PreValue> PreValues
        {
            get
            {
                return new Dictionary<string, PreValue> {
                    {"filter", new PreValue("BlogItem")},
                    {"maxNumber", new PreValue("2")}
                };
            }
        }
    }
}