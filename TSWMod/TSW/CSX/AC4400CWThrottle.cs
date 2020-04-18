using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class AC4400CWThrottle : TSWLever
    {
        public AC4400CWThrottle(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
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
            if (throttle > 0)  // throttle is 0-8
            {
                return Convert.ToSingle(Math.Round(throttle * 8f, 0));
            }

            if (dynamicBrake > 0) // dynamic brake is -1 - -9
            {
                return Convert.ToSingle(Math.Round(-1 - (dynamicBrake * 8f)));
            }

            return 0;
        }

        private char _currentKeyDown = ' ';
    }
}
