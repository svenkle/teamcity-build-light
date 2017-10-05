using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentScheduler;
using RestEase;
using Svenkle.TeamCityBuildLight.Infrastructure.Configuration;
using Svenkle.TeamCityBuildLight.Infrastructure.Light;
using Svenkle.TeamCityBuildLight.Infrastructure.Logger;
using Svenkle.TeamCityBuildLight.Infrastructure.TeamCity;
using State = Svenkle.TeamCityBuildLight.Infrastructure.TeamCity.State;

namespace Svenkle.TeamCityBuildLight
{
    public class Update : IJob
    {
        private readonly Light _light;
        private readonly ILogger _logger;

        public Update(Light light, ILogger logger)
        {
            _light = light;
            _logger = logger;
        }

        public void Execute()
        {
            try
            {
                _logger.Info($"Getting build status for project/s matching {Configuration.ProjectFilter} with build/s matching {Configuration.BuildFilter}");

                using (var httpClient = new HttpClient { BaseAddress = Configuration.Url })
                {
                    var teamCityClient = RestClient.For<ITeamCityClient>(httpClient);
                    teamCityClient.Authorization = new AuthenticationHeaderValue("Basic", Configuration.Credential);

                    var projectCollection = teamCityClient.GetProjects().Result;

                    _logger.Debug($"{projectCollection.Projects.Length} project/s found matching {Configuration.ProjectFilter}");

                    var results = projectCollection.Projects
                        .Where(x => Configuration.ProjectFilter.IsMatch(x.ProjectId) && Configuration.BuildFilter.IsMatch(x.Id))
                        .SelectMany(x => teamCityClient.GetBuildResults(x.Id).Result.BuildResults.SelectMany(y => y.Builds.Builds))
                        .ToList();

                    _logger.Debug($"{results.Count} build/s found matching {Configuration.ProjectFilter} and {Configuration.BuildFilter}");

                    var running = results.Count(x => x.State == State.Running);

                    _logger.Info($"{running} build/s are currently running");

                    var error = results.Count(x => x.Status != Status.Success);

                    _logger.Info($"{error} build/s are currently failing");

                    if (running > 0)
                        _light.Pulse(Color.Yellow);
                    else
                        _light.Off(Color.Yellow);

                    _light.On(error == 0 ? Color.Green : Color.Red);
                    _light.Off(error == 0 ? Color.Red : Color.Green);
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Unable to refresh build status", exception);
                _light.Off();
            }
        }
    }
}