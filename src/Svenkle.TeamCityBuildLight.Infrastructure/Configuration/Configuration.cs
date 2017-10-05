using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Configuration
{
    public class Configuration
    {
        public static Uri Url => new Uri(ConfigurationManager.AppSettings["Url"]);
        public static Regex BuildFilter => new Regex(ConfigurationManager.AppSettings["BuildFilter"], RegexOptions.IgnoreCase);
        public static string Username => ConfigurationManager.AppSettings["Username"];
        public static string Password => ConfigurationManager.AppSettings["Password"];
        public static string Credential => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
        public static int Frequency => int.Parse(ConfigurationManager.AppSettings["Frequency"]);
    }
}