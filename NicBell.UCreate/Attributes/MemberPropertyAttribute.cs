using System;
using System.Runtime.CompilerServices;

namespace NicBell.UCreate.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MemberPropertyAttribute : PropertyAttribute
    {
        public bool CanEdit { get; set; }
        public bool ShowOnProfile { get; set; }

        public MemberPropertyAttribute([CallerMemberName]string name = null) : base(name)
        {
            CanEdit = false;
            ShowOnProfile = false;
        }
    }
}
