using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class BuildResultCollection
    {
        [JsonProperty(PropertyName = "buildType")]
        public BuildResults[] BuildResults { get; set; }
    }
}