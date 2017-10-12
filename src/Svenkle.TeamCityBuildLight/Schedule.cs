using FluentScheduler;

namespace Svenkle.TeamCityBuildLight
{
    public class Schedule : Registry
    {
        public Schedule()
        {
            NonReentrantAsDefault();
            Schedule<Update>().ToRunNow().AndEvery(30).Seconds();
        }
    }
}