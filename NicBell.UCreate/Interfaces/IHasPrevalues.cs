using System.Collections.Generic;
using Umbraco.Core.Models;

namespace NicBell.UCreate.Interfaces
{
    public interface IHasPreValues
    {
        IDictionary<string, PreValue> PreValues { get; }
    }
}
