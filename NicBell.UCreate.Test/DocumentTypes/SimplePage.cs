using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using NicBell.UCreate.Models;
using NicBell.UCreate.Test.Converters;
using System.ComponentModel;
using System.Drawing;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Simple Page",
        Icon = "icon-umbrella color-black",
        AllowedAsRoot = true,
        AllowedTemplates = new[] { "SimplePage" },
        DefaultTemplate = "SimplePage")]
    public class SimplePage : BaseDocType
    {
        public SimplePage(IPublishedContent content)
            : base(content)
        { }


        [Property(Alias = "heading", TypeName = PropertyTypes.Textstring, Description = "Heading for page", Mandatory = true, TabName = "Content")]
        public string Heading { get; set; }

        [TypeConverter(typeof(NiceColorConverter))]
        [Property(Alias = "theme", TypeName = "Nice Color Picker", TabName = "Content")]
        public Color Theme { get; set; }

        [Property(Alias = "testingNumbers", TypeName = PropertyTypes.Numeric, TabName = "Content")]
        public int TestingNumbers { get; set; }

        [Property(Alias = "testingCheck", TypeName = PropertyTypes.TrueFalse, TabName = "Content")]
        public bool TestingCheck { get; set; }
    }
}