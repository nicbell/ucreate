using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
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
    [DocType(Name = "Base")]
    public class Base : BaseDocType
    {
        public Base(IPublishedContent content) : base(content) { }

        [Property(Alias = "metaTitle", TypeName = PropertyTypes.Textstring, Name = "Title", Description = "Meta title", Mandatory = true, TabName = "Meta")]
        public string MetaTitle { get; set; }

        [Property(Alias = "metaDescription", TypeName = PropertyTypes.Textstring, Name = "Description", Description = "Meta description", Mandatory = true, TabName = "Meta")]
        public string MetaDescription { get; set; }
    }
}