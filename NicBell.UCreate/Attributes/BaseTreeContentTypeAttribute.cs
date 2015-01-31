using System;

namespace NicBell.UCreate.Attributes
{
    /// <summary>
    /// Base attribute for tree content
    /// </summary>
    public abstract class BaseTreeContentTypeAttribute : BaseContentTypeAttribute
    {
        /// <summary>
        /// Allow item as root node
        /// </summary>
        public bool AllowedAsRoot { get; set; }

        /// <summary>
        /// Item is a container
        /// </summary>
        public bool IsContainer { get; set; }

        /// <summary>
        /// Aliases of allowed types
        /// </summary>
        public string[] AllowedTypes { get; set; }
    }
}
