using Umbraco.Core.Models;

namespace NicBell.UCreate.Attributes
{
    public class DataTypeAttribute : BaseTypeAttribute
    {
        /// <summary>
        /// Editor Alias, eg: Umbraco.Core.Constants.PropertyEditors.ColorPickerAlias
        /// </summary>
        public string EditorAlias { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public DataTypeDatabaseType DBType { get; set; }
    }
}
