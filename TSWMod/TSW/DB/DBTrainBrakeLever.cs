using System;
using Memory;

namespace TSWMod.TSW.DB
{
    class DBTrainBrakeLever : TSWLever
    {
        public DBTrainBrakeLever(Mem mem, IntPtr hWnd, UIntPtr basePtr) : base(mem, hWnd, basePtr, true)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            var target = 1 - Math.Min(Convert.ToSingle(Math.Round(raw * 10f, 0) / 10f), 1f);
            return target;
        }


        protected override void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown == '"')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_7);
                }
                _currentKeyDown = ';';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_1);
            }
            else
            {
                if (_currentKeyDown == '"')
                {
                    InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_1);
                }
                _currentKeyDown = ';';
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_7);
            }
        }

        protected override void OnSameValue()
        {
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_1);
            InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_7);
        }


        private char _currentKeyDown = ' ';
    }
}
