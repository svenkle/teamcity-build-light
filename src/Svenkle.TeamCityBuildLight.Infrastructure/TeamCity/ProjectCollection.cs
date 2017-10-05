using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class ProjectCollection
    {
        [JsonProperty(PropertyName = "buildType")]
        public Project[] Projects { get; set; }
    }
}