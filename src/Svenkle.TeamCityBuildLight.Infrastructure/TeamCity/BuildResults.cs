using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class BuildResults
    {
        [JsonProperty("builds")]
        public BuildCollection Builds { get; set; }
    }
}