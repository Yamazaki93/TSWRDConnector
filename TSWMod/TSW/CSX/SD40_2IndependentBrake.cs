using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class SD40_2IndependentBrake : TSWLever
    {
        public SD40_2IndependentBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.03)
            {
                return 0.25f;
            }

            return raw * 0.75f + 0.25f;
        }

        protected override void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown == ']')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_6);
                }
                _currentKeyDown = '[';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_4);
            }
            else
            {
                if (_currentKeyDown == '[')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_4);
                }
                _currentKeyDown = ']';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_6);
            }
        }

        protected override void OnSameValue()
        {
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_4);
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_6);
        }


        private char _currentKeyDown = ' ';
    }
}
