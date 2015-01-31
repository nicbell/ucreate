using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Sync
{
    public class DataTypeSync : BaseTypeSync<DataTypeAttribute>
    {
        /// <summary>
        /// Service
        /// </summary>
        public IDataTypeService Service
        {
            get
            {
                return ApplicationContext.Current.Services.DataTypeService;
            }
        }


        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="itemType"></param>
        public override void Save(Type itemType)
        {
            var dataTypes = Service.GetAllDataTypeDefinitions();
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is DataTypeAttribute) as DataTypeAttribute;
            var instance = Activator.CreateInstance(itemType, null);
            var dt = dataTypes.FirstOrDefault(x => x.Key == new Guid(attr.Key)) ?? new DataTypeDefinition(-1, attr.EditorAlias) { Key = new Guid(attr.Key) };

            dt.Name = attr.Name;
            dt.DatabaseType = attr.DBType;
            dt.PropertyEditorAlias = attr.EditorAlias;

            if (instance is IHasPreValues)
                Service.SaveDataTypeAndPreValues(dt, ((IHasPreValues)instance).PreValues);
            else
                Service.Save(dt);
        }


        public IDataTypeDefinition GetDataType(string name)
        {
            return Service.GetAllDataTypeDefinitions().First(x => x.Name == name);
        }
    }
}
