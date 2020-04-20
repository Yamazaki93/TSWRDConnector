using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class EightNotchThrottle : TSWLever
    {
        public EightNotchThrottle(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            return Convert.ToSingle(Math.Round(raw * 8f, 0));
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

        private char _currentKeyDown = ' ';
    }
}
