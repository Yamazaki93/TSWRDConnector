using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBDirectBrakeLever : TSWLever
    {
        public DBDirectBrakeLever(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.35)
            {
                raw = 0;
            }
            else if (raw < 0.65)
            {
                raw = 0.5f;
            }
            else
            {
                raw = 1;
            }

            return 1 - raw;
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
