using GitHubSearch.Models;
using GitHubSearch.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class GitHubServiceTests
{
    private HttpClient GetMockHttpClient(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        handlerMock
           .Protected()
           // Setup the PROTECTED method SendAsync (which HttpClient calls)
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(responseMessage)
           .Verifiable();

        // Create HttpClient with mocked handler
        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task GetGitHubUserAsync_Returns_User_When_Success()
    {
        // Arrange
        var user = new GitHubUser
        {
            Login = "testuser",
            Repos_Url = "https://api.github.com/users/testuser/repos"
        };
        var userJson = JsonConvert.SerializeObject(user);

        // Mock user API response
        var userResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(userJson)
        };

        // Mock repos API response with empty list
        var reposResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[]")
        };

        // Setup HttpMessageHandler to return responses in sequence
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var callCount = 0;

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? userResponse : reposResponse;
            });

        var httpClient = new HttpClient(handlerMock.Object);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyGitHubSearchApp");

        var service = new GitHubServiceTestable(httpClient);

        // Act
        var result = await service.GetGitHubUserAsync("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Login);
        Assert.NotNull(result.GitHubRepos);
        Assert.Empty(result.GitHubRepos);
    }

    [Fact]
    public async Task GetGitHubUserReposAsync_Returns_Top5Repos()
    {
        // Arrange
        var repos = new List<GitHubRepo>
        {
            new GitHubRepo { Name = "Repo1", StargazersCount = 10 },
            new GitHubRepo { Name = "Repo2", StargazersCount = 50 },
            new GitHubRepo { Name = "Repo3", StargazersCount = 30 },
            new GitHubRepo { Name = "Repo4", StargazersCount = 5 },
            new GitHubRepo { Name = "Repo5", StargazersCount = 15 },
            new GitHubRepo { Name = "Repo6", StargazersCount = 20 },
        };
        var reposJson = JsonConvert.SerializeObject(repos);

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(reposJson)
        };

        var httpClient = GetMockHttpClient(response);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyGitHubSearchApp");

        var service = new GitHubServiceTestable(httpClient);

        // Act
        var topRepos = await service.GetGitHubUserReposAsync("https://api.github.com/users/testuser/repos");

        // Assert: top 5 by stargazers descending
        Assert.Equal(5, topRepos.Count);
        Assert.Equal("Repo2", topRepos[0].Name);
        Assert.Equal(50, topRepos[0].StargazersCount);
        Assert.Equal("Repo3", topRepos[1].Name);
        Assert.Equal("Repo6", topRepos[2].Name);
        Assert.Equal("Repo5", topRepos[3].Name);
        Assert.Equal("Repo1", topRepos[4].Name);
    }
}

// Helper subclass to inject HttpClient for testing
public class GitHubServiceTestable : GitHubService
{
    public GitHubServiceTestable(HttpClient httpClient) => _httpClient = httpClient;

    // Expose _httpClient as protected in base class or use reflection, or
    // just make _httpClient protected internal in GitHubService for this.
}