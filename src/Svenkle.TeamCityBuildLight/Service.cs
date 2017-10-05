using System;
using System.ServiceProcess;
using FluentScheduler;
using StructureMap;
using StructureMap.Building.Interception;
using Svenkle.TeamCityBuildLight.Infrastructure.Configuration;
using Svenkle.TeamCityBuildLight.Infrastructure.Light;
using Svenkle.TeamCityBuildLight.Infrastructure.Logger;

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

            var activatorInterceptor = new ActivatorInterceptor<object>((context, o) => OnActivation(context, o));
            var policy = new InterceptorPolicy<object>(activatorInterceptor);

            _container = new Container(x =>
            {
                x.AddRegistry<LoggerRegistry>();
                x.AddRegistry<LightRegistry>();
                x.AddRegistry<ConfigurationRegistry>();
                x.Policies.Interceptors(policy);
            });
            
            JobManager.JobException += ScheduleDomain_UnhandledException;
            JobManager.JobFactory = new JobFactory(_container);
            JobManager.Initialize(new Schedule());
        }

        protected override void OnStop()
        {
            JobManager.Stop();
            _container.GetInstance<Light>()?.Off();
        }

        private static void OnActivation(IContext context, object o)
        {
            // Do not intercept logger as this will create a circular reference
            if (o is ILogger)
                return;

            context.GetInstance<ILogger>()?.Debug("Retrieving {0} from container", o.GetType().Name);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = _container.GetInstance<ILogger>();
            if (e.ExceptionObject is Exception exception)
                logger?.Fatal(exception);
        }

        private static void ScheduleDomain_UnhandledException(JobExceptionInfo obj)
        {
            var logger = _container.GetInstance<ILogger>();
            logger?.Fatal(obj.Exception);
        }
    }
}