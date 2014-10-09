using NicBell.UCreate.Attributes;
using NicBell.UCreate.Constants;

namespace NicBell.UCreate.Test.MediaTypes
{

    [MediaType(
        Name = "Folder With Cover2",
        Icon = "icon-folder color-blue",
        AllowedAsRoot = true,
        IsContainer = true,
        AllowedTypes = new[] { "FolderWithCover", "Image" })]
    public class FolderWithCover2 : FolderWithCover
    {
        [Property(Alias = "coverImage2", TypeName = PropertyTypes.MediaPicker, Description = "Cover image2.", Mandatory = true)]
        public string CoverImage2 { get; set; }
    }


    [MediaType(
        Name = "Folder With Cover",
        Icon = "icon-folder color-blue",
        AllowedAsRoot = true,
        IsContainer = true,
        AllowedTypes = new[] { "FolderWithCover", "Image" })]
    public class FolderWithCover
    {
        [Property(Alias = "coverImage", TypeName = PropertyTypes.MediaPicker, Description = "Cover image.", Mandatory = true)]
        public string CoverImage { get; set; }
    }
}