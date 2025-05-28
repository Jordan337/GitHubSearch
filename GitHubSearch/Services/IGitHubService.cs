using GitHubSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubSearch.Services
{
    public interface IGitHubService
    {
        Task<GitHubUser> GetGitHubUserAsync(string username);
        Task<List<GitHubRepo>> GetGitHubUserReposAsync(string RepoUrl);
    }
}