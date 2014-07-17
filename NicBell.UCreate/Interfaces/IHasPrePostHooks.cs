using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicBell.UCreate.Interfaces
{
    public interface IHasPrePostHooks
    {
        void PreAdd();
        void PostAdd();
    }
}
