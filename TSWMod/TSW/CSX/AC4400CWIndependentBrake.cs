using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

namespace TSWMod.TSW.CSX
{
    class AC4400CWIndependentBrake : TSWLever
    {
        public AC4400CWIndependentBrake(Mem m, IntPtr hWnd, UIntPtr basePtr) : base(m, hWnd, basePtr, false)
        {
        }

        protected override float TargetValuePreTransform(float raw)
        {
            return 0.8f * raw + 0.2f;
        }

        protected override void OnDifferentValue(float diff)
        {
            if (_bailOff)
            {
                return;
            }

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

        public void EngageBailOffIfNeeded()
        {
            if (!_bailOff && CurrentValue.AlmostEquals(0.2f))
            {
                _bailOff = true;
                InputHelpers.KeyDown(HWND, InputHelpers.VirtualKeyStates.VK_OEM_4);
            }
        }

        public void DisengageBailOffIfNeeded()
        {
            if (_bailOff)
            {
                InputHelpers.KeyUp(HWND, InputHelpers.VirtualKeyStates.VK_OEM_4);
                _bailOff = false;
            }
        }

        private char _currentKeyDown = ' ';
        private bool _bailOff;
    }
}
