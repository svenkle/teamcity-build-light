using FluentScheduler;
using Svenkle.TeamCityBuildLight.Infrastructure.Configuration;

namespace Svenkle.TeamCityBuildLight
{
    public class Schedule : Registry
    {
        public Schedule()
        {
            NonReentrantAsDefault();
            Schedule<Update>().ToRunNow().AndEvery(Configuration.Frequency).Seconds();
        }
    }
}