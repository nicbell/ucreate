using Umbraco.Core.Models;

namespace NicBell.UCreate.Attributes
{
    public class CustomDataTypeAttribute : OrderdSyncAttribute
    {
        public string EditorAlias { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public DataTypeDatabaseType DBType { get; set; }
    }
}
