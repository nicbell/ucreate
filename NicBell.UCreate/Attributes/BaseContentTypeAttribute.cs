using System;

namespace NicBell.UCreate.Attributes
{
    /// <summary>
    /// Base attribute for content
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public abstract class BaseContentTypeAttribute : Attribute
    {
        /// <summary>
        /// Name seen by cms user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description seen by cms user
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Umbraco 7 icon name, eg: icon-picture color-blue
        /// </summary>
        public string Icon { get; set; }
    }
}
