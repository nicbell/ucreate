using System;

namespace NicBell.UCreate.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MemberTypeAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
