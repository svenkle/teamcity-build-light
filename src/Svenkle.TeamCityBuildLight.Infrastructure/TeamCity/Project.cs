using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class Project
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}