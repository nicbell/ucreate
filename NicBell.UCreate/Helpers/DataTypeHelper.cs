using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace NicBell.UCreate.Helpers
{
    public class DataTypeHelper
    {
        public DataTypeHelper() { }

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
        /// Saves DataType to DB
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="prevalues"></param>
        /// <param name="overwrite">Overwrite existing DataType</param>
        public void Save(IDataTypeDefinition dataType, IDictionary<string, PreValue> prevalues, bool overwrite = false)
        {
            var dataTypes = Service.GetAllDataTypeDefinitions();

            if (!dataTypes.Any(x => x.Key == dataType.Key))
            {
                Service.SaveDataTypeAndPreValues(dataType, prevalues);
            }
            else if (overwrite)
            {
                //Item already exists do we fetch it and update it
                var existingDataType = dataTypes.First(x => x.Key == dataType.Key);
                existingDataType.Name = dataType.Name;
                existingDataType.PropertyEditorAlias = dataType.PropertyEditorAlias;
                existingDataType.DatabaseType = dataType.DatabaseType;

                Service.SaveDataTypeAndPreValues(existingDataType, prevalues);
            }
        }


        public IDataTypeDefinition GetDataType(string name)
        {
            return Service.GetAllDataTypeDefinitions().First(x => x.Name == name);
        }
    }
}
