using NicBell.UCreate.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NicBell.UCreate.Attributes;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Sync
{
    public abstract class BaseTypeSync<T> where T : Attribute
    {
        private List<Type> _typesToSync;

        /// <summary>
        /// Types to sync
        /// </summary>
        public List<Type> TypesToSync
        {
            get
            {
                if (_typesToSync == null)
                {
                    _typesToSync = ReflectionHelper.GetTypesWithAttribute(typeof(T));

                }

                return _typesToSync;
            }
        }


        /// <summary>
        /// Sync all
        /// </summary>
        public abstract void SyncAll();


        /// <summary>
        /// Maps properties
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="itemType"></param>
        /// <param name="overwrite"></param>
        protected void MapProperties(IContentTypeBase ct, Type itemType)
        {
            foreach (PropertyInfo propInfo in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                PropertyAttribute propAttr = Attribute.GetCustomAttribute(propInfo, typeof(PropertyAttribute), false) as PropertyAttribute;

                if (propAttr != null)
                {
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
}
