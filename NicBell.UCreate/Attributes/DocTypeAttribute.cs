using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicBell.UCreate.Attributes
{
    public class DocTypeAttribute : BaseTreeContentTypeAttribute
    {        
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
