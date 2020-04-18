using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBThrottleLever : TSWLever
    {
        public DBThrottleLever(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            if (raw < 0.08)
            {
                return 0;
            }

            return raw;
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
