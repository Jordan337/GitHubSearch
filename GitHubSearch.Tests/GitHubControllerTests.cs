using GitHubSearch.Controllers;
using GitHubSearch.Models;
using GitHubSearch.Services;
using Moq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

public class GitHubControllerTests
{
    [Fact]
    public async Task SearchPartial_Returns_PartialView_With_Model()
    {
        // Arrange
        var mockService = new Mock<IGitHubService>();
        mockService.Setup(s => s.GetGitHubUserAsync(It.IsAny<string>()))
                   .ReturnsAsync(new GitHubUser { Login = "testuser" });

        var controller = new GitHubController(mockService.Object);

        // Act
        var result = await controller.SearchPartial("testuser") as PartialViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("_GitHubResult", result.ViewName);
        Assert.IsType<GitHubUser>(result.Model);
        var model = result.Model as GitHubUser;
        Assert.Equal("testuser", model.Login);
    }
}