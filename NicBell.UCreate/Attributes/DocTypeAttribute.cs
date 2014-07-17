using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicBell.UCreate.Attributes
{
    public class DocTypeAttribute : OrderdSyncAttribute
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Alias { get; set; }

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

        /// <summary>
        /// List of allowed templates
        /// </summary>
        public string[] AllowedTemplates { get; set; }

        /// <summary>
        /// Default template
        /// </summary>
        public string DefaultTemplate { get; set; }
    }
}
