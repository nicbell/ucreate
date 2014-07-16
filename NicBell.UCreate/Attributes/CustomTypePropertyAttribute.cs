using NicBell.UCreate.Helpers;
using System;
using System.Runtime.CompilerServices;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Attributes
{
    /// <summary>
    /// Makes property as part of the generated type
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CustomTypePropertyAttribute : Attribute
    {
        public string Alias { get; set; }
        public string Name { get;  private set; }
        public string Description { get; set; }
        public bool Mandatory { get; set; }
        public string ValidationRegExp { get; set; }

        public object DefaultValue { get; set; }
        public string TypeName { get; set; }
        public string TabName { get; set; }


        /// <summary>
        /// Set default values
        /// </summary>
        public CustomTypePropertyAttribute([CallerMemberName]string name = null)
        {
            this.Mandatory = false;
            this.ValidationRegExp = "";
            this.Description = "";
            this.DefaultValue = null;
            this.Alias = "";
            this.TypeName = "Textstring";
            this.Name = name;
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
