using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Complex Page",
       Icon = "icon-zip color-blue",
       AllowedAsRoot = true,
       AllowedTemplates = new[] { "SimplePage" },
       DefaultTemplate = "SimplePage")]
    public class ComplexPage : SimplePage
    {
        public ComplexPage(IPublishedContent content)
            : base(content)
        { }


        [Property(Alias = "subheading", Name = "Sub heading", TypeName = PropertyTypes.Textstring, Description = "Subheading for page", Mandatory = true, TabName = "Content")]
        public string SubHeading { get; set; }

        [Property(Alias = "body", Name = "Body", TypeName = PropertyTypes.Richtexteditor, Description = "Subheading for page", Mandatory = true, TabName = "Content")]
        public string Body { get; set; }

        [Property(Alias = "body2", Name = "Body2", TypeName = PropertyTypes.Richtexteditor, Description = "Subheading for page", Mandatory = true, TabName = "Content")]
        public string Body2 { get; set; }

        [Property(Alias = "body3", Name = "Body3", TypeName = PropertyTypes.Richtexteditor, Description = "Subheading for page", Mandatory = true, TabName = "Content")]
        public string Body3 { get; set; }
    }
}