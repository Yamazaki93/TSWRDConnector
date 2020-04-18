using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RailDriverLeverUpdatedEvent : EventArgs
    {
        public RailDriverLeverUpdatedEvent(RailDriverLeverState state)
        {
            RailDriverLeverState = state;
        }

        public RailDriverLeverState RailDriverLeverState { get; }
    }
}
