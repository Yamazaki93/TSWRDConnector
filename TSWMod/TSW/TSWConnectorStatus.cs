using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    enum TSWConnectorStatus
    {
        NotConnected,
        WaitingForGame,
        Calibrating,
        Error,
        Ready
    }
}
