using System;
using System.ServiceProcess;
using FluentScheduler;
using StructureMap;
using Svenkle.TeamCityBuildLight.Infrastructure.Configuration;
using Svenkle.TeamCityBuildLight.Infrastructure.Light;
using Svenkle.TeamCityBuildLight.Infrastructure.Logger;
using Svenkle.TeamCityBuildLight.Infrastructure.TeamCity;

namespace Svenkle.TeamCityBuildLight
{
    internal class Service : ServiceBase
    {
        private static IContainer _container;

        private static void Main()
        {
            var service = new Service();
            if (Environment.UserInteractive)
            {
                service.OnStart(new string[0]);
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
                service.Stop();
            }
            else
            {
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            var container = new Container(x =>
            {
                x.AddRegistry<LoggerRegistry>();
                x.AddRegistry<LightRegistry>();
                x.AddRegistry<ConfigurationRegistry>();
                x.AddRegistry<TeamCityRegistry>();
            });

            container.AssertConfigurationIsValid();
            _container = container;

            JobManager.JobException += ScheduleDomain_UnhandledException;
            JobManager.JobFactory = new JobFactory(_container);
            JobManager.Initialize(new Schedule());
        }

        protected override void OnStop()
        {
            JobManager.Stop();
            _container.GetInstance<Light>()?.Off();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _container.GetInstance<ILogger>()?.Fatal(e.ExceptionObject as Exception);
            _container.GetInstance<Light>()?.Off();
        }

        private static void ScheduleDomain_UnhandledException(JobExceptionInfo obj)
        {
            _container.GetInstance<ILogger>().Fatal(obj.Exception);
            _container.GetInstance<Light>()?.Off();
        }
    }
}