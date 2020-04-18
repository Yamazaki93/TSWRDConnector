using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    class TSWActiveLocomotiveChangedEvent : EventArgs
    {
        public TSWActiveLocomotiveChangedEvent(string locomotiveName)
        {
            LocomotiveName = locomotiveName;
        }

        public string LocomotiveName { get; }
    }
}
