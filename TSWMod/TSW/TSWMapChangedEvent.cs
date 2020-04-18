using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    class TSWMapChangedEvent : EventArgs
    {
        public TSWMapChangedEvent(string mapName)
        {
            MapName = mapName;
        }

        public string MapName { get; set; }
    }
}
