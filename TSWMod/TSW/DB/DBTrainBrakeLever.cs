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

        protected override InputHelpers.VKCodes[] IncreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeIncrease;
        protected override InputHelpers.VKCodes[] DecreaseKeys => KeyboardLayoutManager.Current.AutomaticBrakeDecrease;
    }
}
