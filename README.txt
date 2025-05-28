GitHub User Search App
A simple ASP.NET MVC (.NET Framework) web application that lets you search GitHub users by username, displaying their profile details and top 5 repositories sorted by stargazer count.

Features
Search GitHub users by username using GitHub's public API

Display user’s name, location, avatar, and other info

Show top 5 repositories sorted by stargazers with repo name (linked), description, and star count

Handle invalid inputs and user-not-found scenarios

Unit tests included for controller and service layers

Dependency Injection with Unity container

Technologies
ASP.NET MVC 5 (.NET Framework)

C#

Unity Dependency Injection

xUnit & Moq for unit testing

Newtonsoft.Json for JSON parsing

Bootstrap 5 for UI styling

Getting Started
Prerequisites
Visual Studio 2019 or later

.NET Framework 4.7.2 or compatible

Internet connection to call GitHub API

Setup
Clone this repository:


git clone https://github.com/yourusername/github-user-search.git
Open the solution in Visual Studio.

Restore NuGet packages.

Build the solution.

Running the Application
Set GitHubSearch project as the startup project.

Run the app (F5 or Ctrl+F5).

Navigate to the home page.

Enter a GitHub username in the search box and click Search.

Results will appear below the search box with user info and repositories.

Testing
The solution includes unit tests using xUnit and Moq.

Tests cover controller actions and GitHub API service.

To run tests:

Open Test Explorer in Visual Studio (Test > Test Explorer).

Run all tests or specific test cases.

Notes
This app does not use third-party GitHub API clients; HTTP requests are made manually.

The User-Agent header is set as per GitHub API requirements.

Handles edge cases such as users without repositories or invalid usernames gracefully.