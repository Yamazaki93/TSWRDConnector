namespace TSWMod.RailDriver
{
    class RailDriverLeverState
    {
        public static readonly RailDriverLeverState Invalid = 
            new RailDriverLeverState(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false);
        public RailDriverLeverState(
            float reverser, 
            float throttle, 
            float autoBrake, 
            float independentBrake, 
            float wiper, 
            float light, 
            float reverserTranslated, 
            float throttleTranslated, 
            float dynamicBrakeTranslated, 
            float autoBrakeTranslated,
            bool bailOff)
        {
            Reverser = reverser;
            Throttle = throttle;
            AutoBrake = autoBrake;
            IndependentBrake = independentBrake;
            Wiper = wiper;
            Light = light;
            ReverserTranslated = reverserTranslated;
            ThrottleTranslated = throttleTranslated;
            DynamicBrakeTranslated = dynamicBrakeTranslated;
            AutoBrakeTranslated = autoBrakeTranslated;
            BailOff = bailOff;
        }

        public float Reverser { get; }
        public float Throttle { get; }
        public float AutoBrake { get; }
        public float IndependentBrake { get; }
        public float Wiper { get; }
        public float Light { get; }

        public float ReverserTranslated { get; }    // 0 = Reverse, 0.5 = Neutral, 1 = Forward
        public float ThrottleTranslated { get; }   // 0 - 1
        public float DynamicBrakeTranslated { get; }   // 0 - 1
        public float AutoBrakeTranslated { get; }  // 0 - 1 = Full auto brake range, 2 = emergency

        public bool BailOff { get; }
    }
}