using System;

namespace NicBell.UCreate.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class BaseTypeAttribute : Attribute
    {
        private bool _overwrite = false;
        private int _syncOrder = 1;


        /// <summary>
        /// Sync order, defaults to 1
        /// </summary>
        public int SyncOrder
        {
            get
            {
                return _syncOrder;
            }
            set
            {
                _syncOrder = value;
            }
        }


        /// <summary>
        /// Always overwrite type, defaults to false
        /// </summary>
        public bool Overwrite
        {
            get
            {
                return _overwrite;
            }
            set
            {
                _overwrite = value;
            }
        }
    }
}


