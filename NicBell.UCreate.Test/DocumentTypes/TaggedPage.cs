using NicBell.UCreate.Attributes;
using NicBell.UCreate.Models;
using NicBell.UCreate.Test.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Tagged Page")]
    public class TaggedPage : BaseDocType
    {
        public TaggedPage(IPublishedContent content) : base(content) { }

        //[TypeConverter(typeof(ValueListConverter))]
        [Property(Alias = "tags", TypeName = "Tag Picker", Name = "Tags", Description = "Tags", Mandatory = false, TabName = "Meta")]
        public IEnumerable<string> Tags
        {
            get { return Content.GetPropertyValue<IEnumerable<string>>("tags"); }
        }
    }
}