using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBAFBLever : TSWLever
    {
        public DBAFBLever(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr)
        {
        }
        protected override void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown == 'r')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_R);
                }
                _currentKeyDown = 'f';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_F);
            }
            else
            {
                if (_currentKeyDown == 'f')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_F);
                }
                _currentKeyDown = 'r';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_R);
            }
        }

        protected override void OnSameValue()
        {
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_R);
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_F);
        }


        private char _currentKeyDown = ' ';
    }
}