using System;

namespace NicBell.UCreate.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MemberGroupAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
