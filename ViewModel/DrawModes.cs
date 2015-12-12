using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdBilancer.ViewModel
{
    [Flags]
    enum DrawModes
    {
        Osnap = 2,
        Ortho = 4,
        Grid  = 8,
        Snap = 16
    }
}
