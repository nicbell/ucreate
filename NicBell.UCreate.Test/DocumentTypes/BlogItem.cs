using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Blog Item",
        Icon = "icon-black icon-article",
        AllowedTemplates = new[] { "BlogItem" },
        CompositionTypes = new[] { typeof(TaggedPage) })]
    public class BlogItem : Base
    {
        public BlogItem(IPublishedContent content)
            : base(content)
        { }


        [Property(Alias = "itemTitle", Name = "Item Title", TypeName = PropertyTypes.Textstring, Description = "Title", Mandatory = true, TabName = "Content")]
        public string ItemTitle
        {
            get { return Content.GetPropertyValue<string>("itemTitle"); }
        }

        [Property(Alias = "itemDate", Name = "Item Date", TypeName = PropertyTypes.DatePicker, Description = "Date", Mandatory = true, TabName = "Content")]
        public DateTime ItemDate { get; set; }

        [Property(Alias = "intro", TypeName = PropertyTypes.Richtexteditor, Description = "Intro", Mandatory = true, TabName = "Content")]
        public string Intro
        {
            get { return Content.GetPropertyValue<string>("intro"); }
        }

        [Property(Alias = "body", TypeName = PropertyTypes.Richtexteditor, Description = "Body", Mandatory = true, TabName = "Content")]
        public string Body { get; set; }
    }
}