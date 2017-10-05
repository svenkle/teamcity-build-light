using System.Net.Http.Headers;
using System.Threading.Tasks;
using RestEase;

namespace Svenkle.TeamCityBuildLight.Infrastructure.TeamCity
{
    [Header("Accept", "application/json")]
    public interface ITeamCityClient
    {
        [Header("Authorization")]
        AuthenticationHeaderValue Authorization { get; set; }

        [Get("/httpAuth/app/rest/buildTypes?fields=buildType(id,projectId)")]
        Task<ProjectCollection> GetProjectsAsync();

        [Get("/httpAuth/app/rest/buildTypes?locator=id:{id}&fields=buildType(name,projectId,builds($locator(running:false,canceled:false,status:failure,count:1),build(startDate,finishDate)))")]
        Task<BuildResultCollection> GetMostRecentFailureAsync([Path] string id);

        [Get("/httpAuth/app/rest/buildTypes?locator=id:{id}&fields=buildType(name,projectId,builds($locator(running:false,canceled:false,status:success,count:1),build(startDate,finishDate)))")]
        Task<BuildResultCollection> GetMostRecentSuccessAsync([Path] string id);
        
        [Get("/httpAuth/app/rest/buildTypes?locator=id:{id}&fields=buildType(name,projectId,builds($locator(running:true,canceled:false,count:1),build(startDate,finishDate)))")]
        Task<BuildResultCollection> GetMostRecentRunningAsync([Path] string id);
    }
}
