using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicBell.UCreate.Test.DocumentTypes
{
    [DocType(Name = "BaseFromPoco")]
    public class BaseFromPoco : Foo
    {
        [Property(Alias = nameof(Title), Name = "Title", TypeName = PropertyTypes.Textstring)]
        public string Title { get; set; }
    }

    public class Foo {
        public string Hello { get; set; }
    }
}