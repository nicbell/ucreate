using NicBell.UCreate.Attributes;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Tagged Page")]
    public class TaggedPage : PublishedContentModel
    {
        public TaggedPage(IPublishedContent content) : base(content) { }

        [Property(Alias = "tags", TypeName = "Tag Picker", Name = "Tags", Description = "Tags", Mandatory = false, TabName = "Meta")]
        public IEnumerable<string> Tags
        {
            get { return Content.GetPropertyValue<IEnumerable<string>>("tags"); }
        }
    }
}