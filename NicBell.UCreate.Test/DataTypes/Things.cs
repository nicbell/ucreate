using Archetype.Models;
using Newtonsoft.Json;
using NicBell.UCreate.Attributes;
using NicBell.UCreate.Interfaces;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Test.DataTypes
{
    [DataType(EditorAlias = Archetype.Constants.PropertyEditorAlias,
       Name = "Things",
       Key = "9769f725-2554-4438-9d21-711991d496d2",
       DBType = DataTypeDatabaseType.Nvarchar)]
    public class Things : IHasPreValues
    {
        public IDictionary<string, PreValue> PreValues
        {
            get
            {
                var archetypePreValue = new ArchetypePreValue
                {
                    StartWithAddButton = true,
                    Fieldsets = new List<ArchetypePreValueFieldset> {
                        new ArchetypePreValueFieldset {
                            Alias = "credit",
                            Label = "Credit",
                            LabelTemplate = "{{name}}",
                            Properties = new List<ArchetypePreValueProperty> {
                                new ArchetypePreValueProperty {
                                    Alias = "name",
                                    Label = "Name",
                                    Value = string.Empty,
                                    DataTypeGuid = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByName(Constants.PropertyTypes.Textstring).Key,
                                    Required = false
                                },
                                new ArchetypePreValueProperty {
                                    Alias = "description",
                                    Label = "Description",
                                    Value = string.Empty,
                                    DataTypeGuid = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByName(Constants.PropertyTypes.Richtexteditor).Key,
                                    Required = false
                                }
                            }
                        }
                    }
                };

                return new Dictionary<string, PreValue> {
                    {"archetypeConfig", new PreValue(JsonConvert.SerializeObject(archetypePreValue))}
                };
            }
        }
    }

    public class Thing
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}