using Archetype.Models;
using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using NicBell.UCreate.Test.Converters;
using NicBell.UCreate.Test.DataTypes;
using NicBell.UCreate.Test.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "Home",
        Icon = "icon-black icon-home",
        AllowedAsRoot = true,
        AllowedTemplates = new[] { "Home" },
        AllowedChildTypes = new[] { typeof(BlogIndex) },
        CompositionTypes = new[] { typeof(TaggedPage) })]
    public class Home : Base
    {
        public Home(IPublishedContent content)
            : base(content)
        { }

        //[TypeConverter(typeof(ArchetypeListConverter<Thing>))]
        [Property(Alias = "someThings", Name = "Some Things", TypeName = "Things", TabName = "Things", Mandatory = true)]
        public List<Thing> Things
        {
            get { return Content.GetPropertyValue<ArchetypeModel>("someThings").ToList<Thing>(); }
        }

        [Property(Alias = "someCopy", Name = "Some Copy", TypeName = PropertyTypes.Richtexteditor, TabName = "Content", Mandatory = true)]
        public string SomeCopy
        {
            get { return Content.GetPropertyValue<string>("someCopy"); }
        }

        [TypeConverter(typeof(NiceColorConverter))]
        [Property(Alias = "someColor", Name = "Some Color", TypeName = "Nice Color Picker", TabName = "Content", Mandatory = true)]
        public Color SomeColor {
            get { return Content.GetPropertyValue<Color>("someColor"); }
        }

        //[TypeConverter(typeof(RelatedLinksConverter))]
        [Property(Alias = "someLinks", Name = "Some Links", TypeName = PropertyTypes.RelatedLinks, TabName = "Content", Mandatory = true)]
        public RelatedLinks SomeLinks {
            get { return Content.GetPropertyValue<RelatedLinks>("someLinks"); }
        }

        //[TypeConverter(typeof(IdListConverter))]
        [Property(Alias = "promotedPosts", Name = "Promoted Posts", TypeName = "Blog Post Picker", TabName = "Promoted Posts", Mandatory = false)]
        public List<IPublishedContent> PromotedPostsIds
        {
            get { return Content.GetPropertyValue<List<IPublishedContent>>("promotedPosts"); }
        }


        /// <summary>
        /// Get the data when we need otherwise we may end up querying everything at once and end up with a chicken and egg.
        /// </summary>
        public Lazy<List<BlogItem>> PromotedPosts
        {
            get { return new Lazy<List<BlogItem>>(() => PromotedPostsIds.Select(x => new BlogItem(x)).ToList()); }
        }
    }
}