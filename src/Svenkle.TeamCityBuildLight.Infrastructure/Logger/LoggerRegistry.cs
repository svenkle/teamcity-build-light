using StructureMap;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Logger
{
    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            log4net.Config.XmlConfigurator.Configure();
            For<ILogger>().Use(y => y.ParentType == null ? new Logger(y.RequestedName) : new Logger(y.ParentType)).AlwaysUnique();
        }
    }
}