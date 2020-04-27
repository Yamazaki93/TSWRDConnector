using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSWMod.TSW
{
    class FrenchDefaultKeyboardLayout : USDefaultKeyboardLayout
    {
        public override InputHelpers.VKCodes[] AlerterReset => new[] {InputHelpers.VKCodes.VK_A};
        
        public override InputHelpers.VKCodes[] AutomaticBrakeDecrease => new[] { InputHelpers.VKCodes.VK_M };
        public override InputHelpers.VKCodes[] AutomaticBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_3 };

        public override InputHelpers.VKCodes[] DynamicBrakeDecrease => new[] { InputHelpers.VKCodes.VK_OEM_PERIOD };
        public override InputHelpers.VKCodes[] DynamicBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_2 };

        public override InputHelpers.VKCodes[] IndependentBrakeDecrease => new[] { InputHelpers.VKCodes.VK_OEM_6 };
        public override InputHelpers.VKCodes[] IndependentBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_1 };

        public override InputHelpers.VKCodes[] ReverserIncrease => new[] { InputHelpers.VKCodes.VK_Z };

        public override InputHelpers.VKCodes[] ThrottleIncrease => new[] { InputHelpers.VKCodes.VK_Q };
    }
}
