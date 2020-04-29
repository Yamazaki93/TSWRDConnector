using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    public interface IKeyboardLayout
    {
        InputHelpers.VKCodes[] AlerterReset { get; }
        InputHelpers.VKCodes[] AutomaticBrakeDecrease { get; }
        InputHelpers.VKCodes[] AutomaticBrakeIncrease { get; }
        InputHelpers.VKCodes[] Bell { get; }
        InputHelpers.VKCodes[] CruiseControlDecrease { get; }
        InputHelpers.VKCodes[] CruiseControlIncrease { get; }
        InputHelpers.VKCodes[] CruiseControlToggle { get; }
        InputHelpers.VKCodes[] DynamicBrakeDecrease { get; }
        InputHelpers.VKCodes[] DynamicBrakeIncrease { get; }
        InputHelpers.VKCodes[] EmergencyBrake { get; }
        InputHelpers.VKCodes[] Horn1 { get; }
        InputHelpers.VKCodes[] Horn2 { get; }
        InputHelpers.VKCodes[] IndependentBrakeDecrease { get; }
        InputHelpers.VKCodes[] IndependentBrakeIncrease { get; }
        InputHelpers.VKCodes[] LightDecrease { get; }
        InputHelpers.VKCodes[] LightIncrease { get; }
        InputHelpers.VKCodes[] PZBAck { get; }
        InputHelpers.VKCodes[] PZBOverride { get; }
        InputHelpers.VKCodes[] PZBRelease { get; }
        InputHelpers.VKCodes[] PantographLower { get; }
        InputHelpers.VKCodes[] PantographRaise { get; }
        InputHelpers.VKCodes[] ReverserDecrease { get; }
        InputHelpers.VKCodes[] ReverserIncrease { get; }
        InputHelpers.VKCodes[] Sand { get; }
        InputHelpers.VKCodes[] ThrottleDecrease { get; }
        InputHelpers.VKCodes[] ThrottleIncrease { get; }
        InputHelpers.VKCodes[] WiperDecrease { get; }
        InputHelpers.VKCodes[] WiperIncrease { get; }
    }
}