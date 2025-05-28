using GitHubSearch.Models;
using GitHubSearch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GitHubSearch.Controllers
{
   
    public class GitHubController : Controller
    {   private readonly IGitHubService _gitHubService;
        // Default constructor for when MVC creates the controller
        

        // Constructor for dependency injection
        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }
    

      

        public async Task<ActionResult> Index()
        {
            
            GitHubUser gitHubUser =new GitHubUser();
            return View(gitHubUser);
        }

        [HttpGet]
        public async Task<PartialViewResult> SearchPartial(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return PartialView("_GitHubResult", null);
            }

            var user = await _gitHubService.GetGitHubUserAsync(username);

            return PartialView("_GitHubResult", user);
        }

    }
}