using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RailDriverConnectionChangedEvent : EventArgs
    {
        public RailDriverConnectionChangedEvent(bool connected)
        {
            Connected = connected;
        }

        public bool Connected { get; }
    }
}
