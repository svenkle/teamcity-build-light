using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class Build
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public State State { get; set; }
    }
}