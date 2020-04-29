namespace TSWMod.RailDriver
{
    class RailDriverLeverState
    {
        public static readonly RailDriverLeverState Invalid = 
            new RailDriverLeverState(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0);
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
            bool bailOff, 
            int lightTranslated, 
            int wiperTranslated)
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
            LightTranslated = lightTranslated;
            WiperTranslated = wiperTranslated;
        }

        public float Reverser { get; }
        public float Throttle { get; }
        public float AutoBrake { get; }
        public float IndependentBrake { get; }
        public float Wiper { get; }
        public float Light { get; }

        public float ReverserTranslated { get; }    // 0 = Reverse, 0.5 = Neutral, 1 = Forward
        public float ThrottleTranslated { get; }   // 0 - 1
        public float DynamicBrakeTranslated { get; }   // 0 = off, 0.02 = setup, 0.03 - 1 = brake range
        public float AutoBrakeTranslated { get; }  // 0 - 1 = Full auto brake range, 2 = emergency

        public int LightTranslated { get; }   // 0 = off, 1 = dim, 2 = bright
        public int WiperTranslated { get; }   // 0 = off, 1 = int., 2 = full

        public bool BailOff { get; }
    }
}