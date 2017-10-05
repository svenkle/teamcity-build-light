using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class BuildCollection
    {
        [JsonProperty(PropertyName = "build")]
        public Build[] Builds { get; set; }
    }
}