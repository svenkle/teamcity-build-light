using System.Net.Http;
using System.Net.Http.Headers;
using RestEase;
using StructureMap;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    public class TeamCityRegistry : Registry
    {
        public TeamCityRegistry()
        {
            For<ITeamCityClient>().Use("Creates ITeamCityClient with URL and Credentials", c =>
            {
                var configuration = c.GetInstance<Configuration.Configuration>();
                return RestClient.For<ITeamCityClient>(new HttpClient
                {
                    BaseAddress = configuration.Url,
                    DefaultRequestHeaders =
                     {
                        Authorization = new AuthenticationHeaderValue("Basic", configuration.Credential)
                     }
                });
            })
            .Singleton();
        }
    }
}