using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using NicBell.UCreate.Converters;
using NicBell.UCreate.Test.Converters;
using NicBell.UCreate.Test.DataTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Home",
        Icon = "icon-black icon-home",
        AllowedAsRoot = true,
        AllowedTemplates = new[] { "Home" },
        AllowedTypes = new[] { "BlogIndex" })]
    public class Home : Base
    {
        public Home(IPublishedContent content)
            : base(content)
        { }

        [TypeConverter(typeof(ArchetypeListConverter<Thing>))]
        [Property(Alias = "someThings", Name = "Some Things", TypeName = "Things", TabName = "Content", Mandatory = true)]
        public List<Thing> Things { get; set; }

        [Property(Alias = "someCopy", Name = "Some Copy", TypeName = PropertyTypes.Richtexteditor, TabName = "Content", Mandatory = true)]
        public string SomeCopy { get; set; }

        [TypeConverter(typeof(NiceColorConverter))]
        [Property(Alias = "someColor", Name = "Some Color", TypeName = "Nice Color Picker", TabName = "Content", Mandatory = true)]
        public Color SomeColor { get; set; }

        [TypeConverter(typeof(RelatedLinksConverter))]
        [Property(Alias = "someLinks", Name = "Some Links", TypeName = PropertyTypes.RelatedLinks, TabName = "Content", Mandatory = true)]
        public List<RelatedLink> SomeLinks { get; set; }
    }
}