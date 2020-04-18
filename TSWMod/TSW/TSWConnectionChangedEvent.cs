using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    class TSWConnectionChangedEvent : EventArgs
    {
        public TSWConnectionChangedEvent(TSWConnectorStatus status)
        {
            Status = status;
        }

        public TSWConnectorStatus Status { get; }
    }
}
