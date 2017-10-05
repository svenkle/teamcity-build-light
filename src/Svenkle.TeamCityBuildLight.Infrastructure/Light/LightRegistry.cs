using System.Collections.Generic;
using StructureMap;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Light
{
    public class LightRegistry : Registry
    {
        public LightRegistry()
        {
            For<Light>().Use(new Light(globes: new Dictionary<Color, Globe>
            {
                { Color.Yellow, new Globe(State.Off, Mode.Pulse, 80, 10, 90) },
                { Color.Red, new Globe(State.Off, Mode.Solid, 80, 10, 60) },
                { Color.Green, new Globe(State.Off, Mode.Solid, 80, 10, 60) }
            })).Singleton();
        }
    }
}