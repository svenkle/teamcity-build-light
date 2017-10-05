using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class Build
    {
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? FinishDate { get; set; }
    }
}