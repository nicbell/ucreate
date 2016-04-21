using NicBell.UCreate.Attributes;
using NicBell.UCreate.Models;
using NicBell.UCreate.Test.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Tagged Page")]
    public class TaggedPage : BaseDocType
    {
        public TaggedPage(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ValueListConverter))]
        [Property(Alias = "tags", TypeName = "Tag Picker", Name = "Tags", Description = "Tags", Mandatory = false, TabName = "Meta")]
        public List<string> Tags { get; set; }
    }
}