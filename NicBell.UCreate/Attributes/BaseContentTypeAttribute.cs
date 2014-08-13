using System;

namespace NicBell.UCreate.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class BaseContentTypeAttribute : Attribute
    {
        /// <summary>
        /// Name that user sees
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Umbraco 7 icon name, eg: icon-picture color-blue
        /// </summary>
        public string Icon { get; set; }

        public bool AllowedAsRoot { get; set; }
        public bool IsContainer { get; set; }

        /// <summary>
        /// Aliases of allowed types
        /// </summary>
        public string[] AllowedTypes { get; set; }
    }
}
