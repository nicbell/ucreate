using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicBell.UCreate.Constants
{
    public static class PropertyTypes
    {
        public const string ApprovedColor = "Approved Color";
        public const string Checkboxlist = "Checkbox list";
        public const string ContentPicker = "Content Picker";
        public const string DatePickerwithtime = "Date Picker with time";
        public const string DatePicker = "Date Picker";
        public const string Dropdownmultiple = "Dropdown multiple";
        public const string Dropdown = "Dropdown";
        public const string FolderBrowser = "Folder Browser";
        public const string Label = "Label";
        public const string MediaPicker = "Media Picker";
        public const string MemberPicker = "Member Picker";
        public const string MultipleMediaPicker = "Multiple Media Picker";
        public const string Numeric = "Numeric";
        public const string Radiobox = "Radiobox";
        public const string RelatedLinks = "Related Links";
        public const string Richtexteditor = "Richtext editor";
        public const string Tags = "Tags";
        [Obsolete("From Umbraco 7.4 use PropertyTypes.TextArea")]
        public const string Textboxmultiple = "Textbox multiple";
        public const string Textarea = "Textarea";
        public const string Textstring = "Textstring";
        public const string TrueFalse = "True/false";
        public const string Upload = "Upload";
    }
}
