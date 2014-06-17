using System.Collections.Generic;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Types
{
    public abstract class CustomDataTypeBase
    {
        public CustomDataTypeBase() { }

        public abstract IDictionary<string, PreValue> PreValues { get; }

        public virtual void PreAdd() { }
        public virtual void PostAdd() { }
    }
}
