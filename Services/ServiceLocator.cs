using Microsoft.Extensions.DependencyInjection;
namespace PHILOBM.Services;

public static class ServiceLocator
{
    public static T GetService<T>() where T : class
    {
        var service = App.AppHost?.Services.GetRequiredService<T>() ?? throw new Exception($"{typeof(T).Name} not loaded");

        return service;
    }
}
