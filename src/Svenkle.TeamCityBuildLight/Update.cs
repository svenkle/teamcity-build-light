using System;
using System.Collections.Generic;
using System.Linq;
using FluentScheduler;
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
        private readonly ITeamCityClient _teamCityClient;
        private readonly Configuration _configuration;

        public Update(Light light, ILogger logger, ITeamCityClient teamCityClient, Configuration configuration)
        {
            _light = light;
            _logger = logger;
            _teamCityClient = teamCityClient;
            _configuration = configuration;
        }

        public void Execute()
        {
            try
            {
                _logger.Info($"Checking build statuses for builds matching {_configuration.BuildFilter}");

                var projects = GetProjects();

                _logger.Debug($"{projects.Count} build/s found");

                var error = false;
                var inprogress = false;

                foreach (var project in projects)
                {
                    _logger.Debug($"Getting states for {project}");
                    error = GetErrorState(error, project);
                    inprogress = GetRunningState(inprogress, project);
                }

                SetLightState(error, inprogress);
            }
            catch (Exception exception)
            {
                _logger.Error("Unable to refresh build status", exception);
                _light.Off();
            }
        }

        private List<string> GetProjects()
        {
            return _teamCityClient.GetProjectsAsync().Result.Projects
                .Where(x => _configuration.BuildFilter.IsMatch(x.Id)).Select(x => x.Id).ToList();
        }

        private bool GetRunningState(bool inprogress, string project)
        {
            if (!inprogress)
            {
                _logger.Debug($"Getting running state for {project}");
                var running = _teamCityClient.GetMostRecentRunningAsync(project).Result.Flatten();
                inprogress = running?.StartDate != null;
            }
            else
            {
                _logger.Debug($"Skipping {project} as there is already a build running within search scope");
            }

            return inprogress;
        }

        private bool GetErrorState(bool error, string project)
        {
            if (!error)
            {
                _logger.Debug($"Getting failure state for {project}");
                var failure = _teamCityClient.GetMostRecentFailureAsync(project).Result.Flatten();
                _logger.Debug($"Latest failure start:{failure?.StartDate} end:{failure?.FinishDate}");
                _logger.Debug($"Getting success state for {project}");
                var success = _teamCityClient.GetMostRecentSuccessAsync(project).Result.Flatten();
                _logger.Debug($"Latest success start:{success?.StartDate} end:{success?.FinishDate}");
                error = failure?.FinishDate > success?.StartDate || success?.StartDate == null && failure?.FinishDate != null;
            }
            else
            {
                _logger.Debug($"Skipping {project} as an error has already been detected within search scope");
            }

            return error;
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