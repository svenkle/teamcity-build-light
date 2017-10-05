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
        Task<ProjectCollection> GetProjects();
        
        [Get("/httpAuth/app/rest/buildTypes?locator=id:{id}&fields=buildType(name,projectId,builds($locator(running:any,canceled:false,count:3),build(status,state,branchName)))")]
        Task<BuildResultCollection> GetBuildResults([Path] string id);
    }
}
