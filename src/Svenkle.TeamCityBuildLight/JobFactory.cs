using FluentScheduler;
using StructureMap;

namespace Svenkle.TeamCityBuildLight
{
    public class JobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public JobFactory(IContainer container)
        {
            _container = container;
        }

        public IJob GetJobInstance<T>() where T : IJob
        {
            return _container.GetInstance<T>();
        }
    }
}