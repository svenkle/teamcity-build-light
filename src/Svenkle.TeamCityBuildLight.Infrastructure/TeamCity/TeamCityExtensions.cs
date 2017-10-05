using System.Linq;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public static class TeamCityExtensions
    {
        public static Build Flatten(this BuildResultCollection buildResultCollection)
        {
            return buildResultCollection?.BuildResults?.FirstOrDefault()?.Builds?.Builds?.FirstOrDefault();
        }
    }
}