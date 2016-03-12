using NicBell.UCreate.Sync;
using System;
using System.Runtime.CompilerServices;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Attributes
{
    /// <summary>
    /// Makes property as part of the generated type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Mandatory { get; set; }
        public string ValidationRegExp { get; set; }
        public string TabName { get; set; }
        //public object DefaultValue { get; set; }

        /// <summary>
        /// Property Type. Tip: for standard types use constants in "NicBell.UCreate.Constants.PropertyTypes"
        /// </summary>
        public string TypeName { get; set; }
        
        /// <summary>
        /// Set default values
        /// </summary>
        public PropertyAttribute([CallerMemberName]string name = null)
        {
            this.Mandatory = false;
            this.ValidationRegExp = "";
            this.Description = "";
            //this.DefaultValue = null;
            this.Alias = "";
            this.TypeName = "Textstring";
            this.Name = name;
        }


        public PropertyType GetPropertyType()
        {
            var dataType = new DataTypeSync().GetDataType(TypeName);
            if(dataType == null)
                throw new Exception($"Failed to find data type with name: {TypeName} for property: {Name}");

            var propType = new PropertyType(dataType)
            {
                Alias = Alias,
                Name = Name,
                Description = Description,
                Mandatory = Mandatory,
                ValidationRegExp = ValidationRegExp
            };

            return propType;
        }
    }
}
