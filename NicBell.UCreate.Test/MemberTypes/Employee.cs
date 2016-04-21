using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;

namespace NicBell.UCreate.Test.MemberTypes
{
    [MemberType(Name = "Employee", Description = "Member who represents an employee", Icon = "icon-user color-green")]
    public class Employee
    {
        [MemberProperty(Alias = "jobTitle", TypeName = PropertyTypes.Textstring, Description = "Employee's job title", Mandatory = true, TabName = "Job Details", CanEdit = true, ShowOnProfile = true)]
        public string JobTitle { get; set; }

        [MemberProperty(Alias = "jobDescription", TypeName = PropertyTypes.Textarea, Description = "Employee's job description", Mandatory = false, TabName = "Job Details", CanEdit = true, ShowOnProfile = false)]
        public string JobDescription { get; set; }

        [MemberProperty(Alias = "profilePicture", TypeName = PropertyTypes.MediaPicker, Description = "Admin profile picture", Mandatory = false, CanEdit = true, ShowOnProfile = true)]
        public string ProfilePicture { get; set; }
    }
}