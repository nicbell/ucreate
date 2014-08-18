using NicBell.UCreate.Helpers;
using System;
using System.Collections.Generic;

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
    }
}
