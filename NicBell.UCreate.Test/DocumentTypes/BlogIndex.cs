using NicBell.UCreate.Attributes;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Blog Index",
        Icon = "icon-black icon-bulleted-list",
        AllowedTemplates = new[] { "BlogIndex" },
        AllowedChildTypes = new[] { typeof(BlogItem) })]
    public class BlogIndex : Base
    {
        public BlogIndex(IPublishedContent content)
            : base(content)
        { }
    }
}