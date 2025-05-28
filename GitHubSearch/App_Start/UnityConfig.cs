using GitHubSearch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

public static class UnityConfig
{
    private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
    {
        var container = new UnityContainer();
        RegisterTypes(container);
        return container;
    });

    public static IUnityContainer Container => container.Value;

    public static void RegisterTypes(IUnityContainer container)
    {
        // Register your types here
        // e.g. container.RegisterType<IProductRepository, ProductRepository>();

        container.RegisterType<IGitHubService, GitHubService>();
    }
}