using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.NEC
{
    // 1 = full throttle
    // -1 = full dynamic brake
    class ACSMasterController : TSWLever
    {
        public ACSMasterController(Mem m, IntPtr hWnd, UIntPtr basePtr, bool hasNotch = false) : base(m, hWnd, basePtr, hasNotch)
        {
        }

        protected override void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown == 'a')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_A);
                }
                _currentKeyDown = 'd';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_D);
            }
            else
            {
                if (_currentKeyDown == 'd')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_D);
                }
                _currentKeyDown = 'a';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_A);
            }
        }

        protected override void OnSameValue()
        {
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_D);
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_A);
        }

        public float TranslateCombinedValue(float throttle, float dynamicBrake)
        {
            if (throttle > 0)  // throttle is 0-1 continuous
            {
                return throttle;
            }

            if (dynamicBrake > 0) // dynamic brake is 0 - -1 continuous
            {
                return -1 * dynamicBrake;
            }

            return 0;
        }

        private char _currentKeyDown = ' ';
    }
}
