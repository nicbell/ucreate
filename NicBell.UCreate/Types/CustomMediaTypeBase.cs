using System.Collections.Generic;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Types
{
    public class CustomMediaTypeBase
    {
        public CustomMediaTypeBase() { }

        public virtual IList<KeyValuePair<string, PropertyType>> Properties
        {
            get
            {
                return new List<KeyValuePair<string, PropertyType>>();
            }
        }

        public virtual void PreAdd() { }
        public virtual void PostAdd() { }
    }
}
