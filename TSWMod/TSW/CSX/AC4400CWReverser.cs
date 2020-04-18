using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class AC4400CWReverser : TSWLever
    {
        public AC4400CWReverser(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, true)
        {
            NotchRampUp = 30;
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw.AlmostEquals(1))
            {
                raw = 1;
            }
            else if (raw.AlmostEquals(0.5f))
            {
                raw = 0;
            }
            else
            {
                raw = -1;
            }

            return raw;
        }

        protected override void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown == 'w')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_W);
                }
                _currentKeyDown = 's';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_S);
            }
            else
            {
                if (_currentKeyDown == 's')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_S);
                }
                _currentKeyDown = 'w';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_W);
            }
        }

        protected override void OnSameValue()
        {
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_W);
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_S);
        }

        private char _currentKeyDown = ' ';

    }
}
