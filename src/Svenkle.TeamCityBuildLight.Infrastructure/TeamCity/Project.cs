using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}