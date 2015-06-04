using NicBell.UCreate.Attributes;
using System;
using System.Linq;
using System.Reflection;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Sync
{
    public abstract class BaseContentTypeSync<T> : BaseTypeSync<T> where T : BaseContentTypeAttribute
    {
        /// <summary>
        /// Maps properties
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="itemType"></param>
        /// <param name="overwrite"></param>
        protected void MapProperties(IContentTypeBase ct, Type itemType)
        {
            foreach (PropertyInfo propInfo in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(prop => Attribute.IsDefined(prop, typeof(PropertyAttribute), false)))
            {
                var propAttr = propInfo.GetCustomAttribute<PropertyAttribute>(false);
                var newProp = propAttr.GetPropertyType();

                if (ct.PropertyTypeExists(propAttr.Alias))
                {
                    var existingProp = ct.PropertyTypes.First(x => x.Alias == propAttr.Alias);

                    existingProp.DataTypeDefinitionId = newProp.DataTypeDefinitionId;
                    existingProp.Name = newProp.Name;
                    existingProp.Description = newProp.Description;
                    existingProp.Mandatory = newProp.Mandatory;
                    existingProp.ValidationRegExp = newProp.ValidationRegExp;

                    if (!String.IsNullOrEmpty(propAttr.TabName))
                    {
                        ct.MovePropertyType(propAttr.Alias, propAttr.TabName);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(propAttr.TabName))
                    {
                        ct.AddPropertyGroup(propAttr.TabName);
                        ct.AddPropertyType(newProp, propAttr.TabName);
                    }
                    else
                    {
                        ct.AddPropertyType(newProp);
                    }
                }
            }
        }
    }
}
