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
                _logger.Info($"Checking build statuses for builds matching {Configuration.BuildFilter}");

                using (var httpClient = new HttpClient { BaseAddress = Configuration.Url })
                {
                    var teamCityClient = RestClient.For<ITeamCityClient>(httpClient);
                    teamCityClient.Authorization = new AuthenticationHeaderValue("Basic", Configuration.Credential);

                    var projects = teamCityClient.GetProjectsAsync().Result.Projects
                        .Where(x => Configuration.BuildFilter.IsMatch(x.Id)).Select(x => x.Id).ToList();

                    _logger.Debug($"{projects.Count} build/s found");

                    var error = false;
                    var inprogress = false;

                    foreach (var project in projects)
                    {
                        _logger.Debug($"Getting latest failure for {project}");
                        var failure = teamCityClient.GetMostRecentFailureAsync(project).Result.Flatten();
                        _logger.Debug($"Latest failure start:{failure.StartDate} end:{failure.FinishDate}");

                        _logger.Debug($"Getting latest success for {project}");
                        var success = teamCityClient.GetMostRecentSuccessAsync(project).Result.Flatten();
                        _logger.Debug($"Latest success start:{success.StartDate} end:{success.FinishDate}");

                        _logger.Debug($"Getting latest running build for {project}");
                        var running = teamCityClient.GetMostRecentRunningAsync(project).Result.Flatten();

                        if (!inprogress)
                            inprogress = running?.StartDate != null;

                        if (!error)
                            error = failure.FinishDate > success.StartDate;
                    }

                    SetLightState(error, inprogress);
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Unable to refresh build status", exception);
                _light.Off();
            }
        }

        private void SetLightState(bool error, bool running)
        {
            _light.On(error ? Color.Red : Color.Green);
            _light.Off(error ? Color.Green : Color.Red);

            if (running)
                _light.Pulse(Color.Yellow);
            else
                _light.Off(Color.Yellow);
        }
    }
}