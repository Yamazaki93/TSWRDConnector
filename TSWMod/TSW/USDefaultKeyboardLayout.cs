using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    class USDefaultKeyboardLayout : IKeyboardLayout
    {
        public virtual InputHelpers.VKCodes[] AlerterReset => new[] { InputHelpers.VKCodes.VK_Q };
        public virtual InputHelpers.VKCodes[] AutomaticBrakeDecrease => new[] { InputHelpers.VKCodes.VK_OEM_1 };
        public virtual InputHelpers.VKCodes[] AutomaticBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_7 };
        public virtual InputHelpers.VKCodes[] Bell => new[] { InputHelpers.VKCodes.VK_B };
        public virtual InputHelpers.VKCodes[] CruiseControlDecrease => new[] { InputHelpers.VKCodes.VK_F };
        public virtual InputHelpers.VKCodes[] CruiseControlIncrease => new[] { InputHelpers.VKCodes.VK_R };
        public virtual InputHelpers.VKCodes[] CruiseControlToggle => new[] { InputHelpers.VKCodes.VK_LCONTROL, InputHelpers.VKCodes.VK_R };
        public virtual InputHelpers.VKCodes[] DynamicBrakeDecrease => new[] {InputHelpers.VKCodes.VK_OEM_COMMA};
        public virtual InputHelpers.VKCodes[] DynamicBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_PERIOD };
        public virtual InputHelpers.VKCodes[] EmergencyBrake => new[] { InputHelpers.VKCodes.VK_BACK };
        public virtual InputHelpers.VKCodes[] Horn1 => new[] { InputHelpers.VKCodes.VK_SPACE };
        public virtual InputHelpers.VKCodes[] Horn2 => new[] { InputHelpers.VKCodes.VK_N };
        public virtual InputHelpers.VKCodes[] IndependentBrakeDecrease => new[] { InputHelpers.VKCodes.VK_OEM_4 };
        public virtual InputHelpers.VKCodes[] IndependentBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_6 };
        public InputHelpers.VKCodes[] LightDecrease => new[] { InputHelpers.VKCodes.VK_LSHIFT, InputHelpers.VKCodes.VK_H };
        public InputHelpers.VKCodes[] LightIncrease => new[] { InputHelpers.VKCodes.VK_H };
        public virtual InputHelpers.VKCodes[] PZBAck => new[] { InputHelpers.VKCodes.VK_NEXT };
        public virtual InputHelpers.VKCodes[] PZBOverride => new[] { InputHelpers.VKCodes.VK_DELETE };
        public virtual InputHelpers.VKCodes[] PZBRelease => new[] { InputHelpers.VKCodes.VK_END };
        public virtual InputHelpers.VKCodes[] PantographLower => new[] { InputHelpers.VKCodes.VK_LSHIFT, InputHelpers.VKCodes.VK_P };
        public virtual InputHelpers.VKCodes[] PantographRaise => new[] { InputHelpers.VKCodes.VK_P };
        public virtual InputHelpers.VKCodes[] ReverserDecrease => new[] { InputHelpers.VKCodes.VK_S };
        public virtual InputHelpers.VKCodes[] ReverserIncrease => new[] { InputHelpers.VKCodes.VK_W };
        public virtual InputHelpers.VKCodes[] Sand => new[] { InputHelpers.VKCodes.VK_X };
        public virtual InputHelpers.VKCodes[] ThrottleDecrease => new[] { InputHelpers.VKCodes.VK_D };
        public virtual InputHelpers.VKCodes[] ThrottleIncrease => new[] { InputHelpers.VKCodes.VK_A };
        public InputHelpers.VKCodes[] WiperDecrease => new[] { InputHelpers.VKCodes.VK_LSHIFT, InputHelpers.VKCodes.VK_V };
        public InputHelpers.VKCodes[] WiperIncrease => new[] { InputHelpers.VKCodes.VK_V };
    }
}