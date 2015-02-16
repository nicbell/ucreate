using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using System;
using System.Collections.Generic;
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
            var attr = Attribute.GetCustomAttributes(itemType).FirstOrDefault(x => x is DataTypeAttribute) as DataTypeAttribute;
            var instance = Activator.CreateInstance(itemType, null);
            var dt = Service.GetDataTypeDefinitionById(new Guid(attr.Key)) ?? new DataTypeDefinition(-1, attr.EditorAlias) { Key = new Guid(attr.Key) };

            dt.Name = attr.Name;
            dt.DatabaseType = attr.DBType;
            dt.PropertyEditorAlias = attr.EditorAlias;

            if (instance is IHasPreValues)
            {
                Service.SaveDataTypeAndPreValues(dt, MergePreValues(dt, ((IHasPreValues) instance).PreValues));
            }
            else
            {
                Service.Save(dt);
            }
        }


        private IDictionary<string, PreValue> MergePreValues(IDataTypeDefinition dt, IDictionary<string, PreValue> newPrevalues)
        {
            var mergedPrevalues = new Dictionary<string, PreValue>();

            if (!dt.HasIdentity)
            {
                return newPrevalues;
            }
            
            var oldPrevalues = Service.GetPreValuesCollectionByDataTypeId(dt.Id).PreValuesAsDictionary;

            foreach (var preValue in newPrevalues)
            {
                var id = oldPrevalues.ContainsKey(preValue.Key)
                    ? oldPrevalues[preValue.Key].Id
                    : preValue.Value.Id;

                mergedPrevalues.Add(preValue.Key, new PreValue(id, preValue.Value.Value, preValue.Value.SortOrder));
            }

            return mergedPrevalues;
        }


        /// <summary>
        /// Gets DataTypeDefinition by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDataTypeDefinition GetDataType(string name)
        {
            return Service.GetDataTypeDefinitionByName(name);
        }
    }
}
