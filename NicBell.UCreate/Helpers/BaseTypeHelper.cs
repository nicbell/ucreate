using System;
using System.Collections.Generic;
using System.Linq;

namespace NicBell.UCreate.Helpers
{
    public abstract class BaseTypeHelper<T> where T : Attribute
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
                    var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
                    _typesToSync = new List<Type>();

                    foreach (Type t in allTypes)
                    {
                        var attr = Attribute.GetCustomAttribute(t, typeof(T));

                        if (attr != null)
                            _typesToSync.Add(t);
                    }
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
