using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class SD40_2DynamicBrake : TSWLever
    {
        public SD40_2DynamicBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.01)
            {
                return 0;
            }

            if (raw.AlmostEquals(0.02f))
            {
                return 0.14f;  //initial setup
            }

            return raw * 0.8f + 0.2f;
        }

        protected override void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown == '.')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_PERIOD);
                }
                _currentKeyDown = ',';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_COMMA);
            }
            else
            {
                if (_currentKeyDown == ',')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_COMMA);
                }
                _currentKeyDown = '.';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_PERIOD);
            }
        }

        protected override void OnSameValue()
        {
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_PERIOD);
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_COMMA);
        }


        private char _currentKeyDown = ' ';
    }
}
