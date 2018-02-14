using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using NicBell.UCreate.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Base")]
    public class Base : BaseDocType
    {
        public Base(IPublishedContent content) : base(content) { }

        [Property(Alias = "metaTitle", TypeName = PropertyTypes.Textstring, Name = "Title", Description = "Meta title", Mandatory = true, TabName = "Meta")]
        public string MetaTitle
        {
            get { return Content.GetPropertyValue<string>("metaTitle"); }
        }

        [Property(Alias = "metaDescription", TypeName = PropertyTypes.Textstring, Name = "Description", Description = "Meta description", Mandatory = true, TabName = "Meta")]
        public string MetaDescription
        {
            get { return Content.GetPropertyValue<string>("metaDescription"); }
        }
    }
}