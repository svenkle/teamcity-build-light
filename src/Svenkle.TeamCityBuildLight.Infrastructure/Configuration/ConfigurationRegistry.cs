using StructureMap;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Configuration
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<Configuration>().Use<Configuration>().Singleton();
        }
    }
}