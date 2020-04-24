using System;
using System.Collections.Generic;
using Memory;
using Newtonsoft.Json.Linq;
using TSWMod.RailDriver;

namespace TSWMod.TSW
{
    interface ILocomotive
    {
        bool CheckPlayerCalibration();
        void OnControlLoop(RailDriverLeverState state, int[] pressedButtons);
        void Close();
        void SetConfiguration(JObject config);
        IDictionary<int, InputHelpers.VirtualKeyStates[]> GetButtonMappings();
        JObject GetConfiguration();
        string Name { get; }
    }
}
