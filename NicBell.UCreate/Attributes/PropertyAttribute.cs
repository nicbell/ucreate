using NicBell.UCreate.Helpers;
using NicBell.UCreate.Interfaces;
using System;
using System.Runtime.CompilerServices;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Attributes
{
    /// <summary>
    /// Makes property as part of the generated type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyAttribute : Attribute
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Mandatory { get; set; }
        public string ValidationRegExp { get; set; }
        //public object DefaultValue { get; set; }

        /// <summary>
        /// Property Type. Tip: for standard types use constants in "NicBell.UCreate.Constants.PropertyTypes"
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// If you want to convert the stored value to a custom type
        /// </summary>
        public Type TypeConverter { get; set; }
        public string TabName { get; set; }

        
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
            this.TypeConverter = null;
        }


        public PropertyType GetPropertyType()
        {
            var propType = new PropertyType(new DataTypeHelper().GetDataType(TypeName))
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
