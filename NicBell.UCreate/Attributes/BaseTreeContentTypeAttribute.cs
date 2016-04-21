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
        /// Item is a container, enables list view
        /// </summary>
        public bool IsContainer { get; set; }

        /// <summary>
        /// Allowed child node types
        /// </summary>
        public Type[] AllowedChildTypes { get; set; }

        /// <summary>
        /// Composition types
        /// </summary>
        public Type[] CompositionTypes { get; set; }
    }
}
