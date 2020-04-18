using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.RailDriver
{
    class RailDriverButtonEvent : EventArgs
    {
        public RailDriverButtonEvent(int keyCode)
        {
            KeyCode = keyCode;
        }

        public int KeyCode { get; }
    }
}
