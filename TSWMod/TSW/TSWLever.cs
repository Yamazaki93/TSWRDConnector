using System;
using System.Linq;
using Memory;

namespace TSWMod.TSW
{
    abstract class TSWLever
    {
        protected const ulong AbsoluteValueOffset = 0x04F0;   // Absolute lever value offset
        protected const ulong AdjustedValueOffset = 0x04F4;   // Absolute lever value offset
        protected const ulong SpeedMultiplierOffset = 0x051C;   // Adjustment speed multiplier?


        protected TSWLever(Mem m, IntPtr hWnd, UIntPtr basePtr, bool isNotch = false)
        {
            _m = m;
            _hWnd = hWnd;
            _basePtr = basePtr;
            _isNotch = isNotch;
        }

        public virtual void OnControlLoop(float targetValue)
        {
            // For levers with notch, cool off and ramp up is needed to prevent "ticking"
            if (_coolingOff && _coolOffCounter > 0)
            {
                _coolOffCounter--;
                return;
            }
            if (_coolingOff && _coolOffCounter == 0)
            {
                _coolingOff = false;
            }

            if (_rampingUp && _rampUpTimer > 0)
            {
                _rampUpTimer--;
                return;
            }
            if (_rampingUp && _rampUpTimer == 0)
            {
                _rampingUp = false;
            }


            // check current TSW value against target value
            var currentValue = CurrentValuePreTransform(CurrentValue);
            targetValue = TargetValuePreTransform(targetValue);
            var currentDiff = targetValue - currentValue;
            if (targetValue.AlmostEquals(currentValue) && _isChanging)  // if value is ok
            {
                _isChanging = false;
                _coolingOff = _isNotch;
                _coolOffCounter = NotchCoolOff;
                OnSameValue();
            }
            else if (!targetValue.AlmostEquals(currentValue)) // if value not ok
            {
                _isChanging = true;
                _rampingUp = _isNotch;
                _rampUpTimer = NotchRampUp;
                OnDifferentValue(currentDiff);
            }

            if (_previousDiff * currentDiff < 0) // sign change on diff value, bouncing
            {
                _isChanging = false;
                _coolingOff = _isNotch;
                _coolOffCounter = NotchCoolOff;
                OnSameValue();
            }
            _previousDiff = currentDiff;
        }

        protected virtual float TargetValuePreTransform(float raw)
        {
            return raw;
        }

        protected virtual float CurrentValuePreTransform(float raw)
        {
            return raw;
        }

        // diff = target - current
        protected virtual void OnDifferentValue(float diff)
        {
            if (diff < 0)
            {
                if (_currentKeyDown.SequenceEqual(IncreaseKeys))
                {
                    InputHelpers.KeyComboUp(_hWnd, IncreaseKeys);
                }
                _currentKeyDown = DecreaseKeys;
                InputHelpers.KeyComboDown(_hWnd, DecreaseKeys);
            }
            else
            {
                if (_currentKeyDown.SequenceEqual(DecreaseKeys))
                {
                    InputHelpers.KeyComboUp(_hWnd, DecreaseKeys);
                }
                _currentKeyDown = IncreaseKeys;
                InputHelpers.KeyComboDown(_hWnd, IncreaseKeys);
            }
        }

        protected virtual void OnSameValue()
        {
            InputHelpers.KeyComboUp(_hWnd, IncreaseKeys);
            InputHelpers.KeyComboUp(_hWnd, DecreaseKeys);
        }

        protected abstract InputHelpers.VKCodes[] IncreaseKeys { get; }
        protected abstract InputHelpers.VKCodes[] DecreaseKeys { get; }

        public float CurrentValue => _isNotch ?
            Convert.ToSingle(Math.Round(_m.ReadFloat(_m.GetCodeRepresentation((UIntPtr)((ulong)_basePtr + AdjustedValueOffset))), 2)):
            _m.ReadFloat(_m.GetCodeRepresentation((UIntPtr)((ulong)_basePtr + AbsoluteValueOffset)));
        protected IntPtr HWND => _hWnd;

        protected int NotchCoolOff = 7;  // A cool off period for notch like lever
        protected int NotchRampUp = 0;  // Ramp up period for notch like lever (for slow notches)

        private InputHelpers.VKCodes[] _currentKeyDown = { };
        private float _previousDiff;
        private readonly Mem _m;
        private readonly IntPtr _hWnd;
        private readonly UIntPtr _basePtr;
        private readonly bool _isNotch;
        private bool _isChanging;
        private bool _coolingOff;
        private int _coolOffCounter;
        private bool _rampingUp;
        private int _rampUpTimer;
    }
}
