namespace TSWMod.TSW
{
    class GBDefaultKeyboardLayout : USDefaultKeyboardLayout
    {
        public override InputHelpers.VKCodes[] AutomaticBrakeIncrease => new[] { InputHelpers.VKCodes.VK_OEM_3 };
        public override InputHelpers.VKCodes[] HandbrakeToggle => new[] { InputHelpers.VKCodes.VK_OEM_2 };

    }
}
