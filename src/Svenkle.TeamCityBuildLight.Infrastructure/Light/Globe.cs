namespace Svenkle.TeamCityBuildLight.Infrastructure.Light
{
    public class Globe
    {
        public State State { get; private set; }
        public Mode Mode { get; private set; }
        public int Brightness { get; private set; }
        public int MinBrightness { get; }
        public int MaxBrightness { get; }

        private int _brightnessIncrement = 1;
        private int _flashCounter;

        public Globe(State state = State.On, Mode mode = Mode.Solid, int brightness = 80, int minBrightness = 10, int maxBrightness = 80)
        {
            State = state;
            Mode = mode;
            MinBrightness = minBrightness;
            MaxBrightness = maxBrightness;
            Brightness = brightness;
        }

        public void Update()
        {
            if (Mode == Mode.Pulse)
            {
                Brightness = Brightness < MinBrightness ? MinBrightness : Brightness > MaxBrightness ? MaxBrightness : Brightness;

                if (Brightness == MaxBrightness || Brightness == MinBrightness)
                {
                    _brightnessIncrement *= -1;
                }

                Brightness += _brightnessIncrement;
            }
            else if (Mode == Mode.Flash)
            {
                if (_flashCounter % 10 == 0)
                {
                    _flashCounter = 0;

                    if (Brightness <= MinBrightness)
                        Brightness = MaxBrightness;
                    else if (Brightness >= MaxBrightness)
                        Brightness = MinBrightness;
                }

                _flashCounter++;
            }
            else
            {
                Brightness = MaxBrightness;
            }
        }

        public void Off()
        {
            State = State.Off;
        }
        
        public void On()
        {
            State = State.On;
        }
        
        public void Solid()
        {
            State = State.On;
            Mode = Mode.Solid;
        }

        public void Pulse()
        {
            State = State.On;
            Mode = Mode.Pulse;
        }

        public void Flash()
        {
            State = State.On;
            Mode = Mode.Flash;
            Brightness = MinBrightness;
        }
    }
}