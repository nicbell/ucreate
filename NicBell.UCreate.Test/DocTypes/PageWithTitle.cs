using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using NicBell.UCreate.Models;
using System.Drawing;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DocTypes
{
    [DocType(Name = "Page With Title",
        Icon = "icon-zip color-blue",
        AllowedAsRoot = true,
        AllowedTemplates = new[] { "PageWithTitle" },
        DefaultTemplate = "PageWithTitle")]
    public class PageWithTitle : BaseDocType
    {
        public PageWithTitle(IPublishedContent content)
            : base(content)
        { }


        [Property(Alias = "heading", TypeName = PropertyTypes.Textstring, Description = "Heading for page", Mandatory = true, TabName = "Content")]
        public string Heading { get; set; }

        [Property(Alias = "theme", TypeName = "Nice Color Picker", TypeConverter = typeof(NiceColorConverter), TabName = "Content")]
        public Color Theme { get; set; }
    }


    [DocType(Name = "Page With Sub Title",
        Icon = "icon-zip color-blue",
        AllowedAsRoot = true,
        AllowedTemplates = new[] { "PageWithTitle" },
        DefaultTemplate = "PageWithTitle")]
    public class PageWithSubtitle : PageWithTitle
    {
        public PageWithSubtitle(IPublishedContent content)
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