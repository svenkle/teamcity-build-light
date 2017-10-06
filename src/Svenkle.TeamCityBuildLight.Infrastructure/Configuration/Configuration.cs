using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Configuration
{
    public class Configuration
    {
        public Uri Url => new Uri(ConfigurationManager.AppSettings["Url"]);
        public Regex BuildFilter => new Regex(ConfigurationManager.AppSettings["BuildFilter"], RegexOptions.IgnoreCase);
        public string Username => ConfigurationManager.AppSettings["Username"];
        public string Password => ConfigurationManager.AppSettings["Password"];
        public string Credential => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
    }
}