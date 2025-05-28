using GitHubSearch.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubSearch.Services
{
    public class GitHubService : IGitHubService
    {
        protected  HttpClient _httpClient;

        public GitHubService() : this(new HttpClient())
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyGitHubSearchApp");
        }

        public GitHubService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyGitHubSearchApp");
        }

        public async Task<GitHubUser> GetGitHubUserAsync(string username)
        {
            var userResponse = await _httpClient.GetAsync($"https://api.github.com/users/{username}");
            var user = new GitHubUser();

            if (userResponse.IsSuccessStatusCode)
            {
                var json = await userResponse.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<GitHubUser>(json);

                // Fetch repos only if repos_url is available
                if (!string.IsNullOrEmpty(user.Repos_Url))
                {
                    user.GitHubRepos = await GetGitHubUserReposAsync(user.Repos_Url);
                }
            }

            return user;
        }

        public async Task<List<GitHubRepo>> GetGitHubUserReposAsync(string repoUrl)
        {
            var reposResponse = await _httpClient.GetAsync(repoUrl);

            if (!reposResponse.IsSuccessStatusCode)
            {
                return new List<GitHubRepo>();
            }

            var reposJson = await reposResponse.Content.ReadAsStringAsync();
            var allRepos = JsonConvert.DeserializeObject<List<GitHubRepo>>(reposJson);

            var topRepos = allRepos
                .OrderByDescending(r => r.StargazersCount)
                .Take(5)
                .ToList();

            return topRepos;
        }
    }
}