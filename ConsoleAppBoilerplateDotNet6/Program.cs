using ConsoleAppBoilerplateDotNet6;using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

public class Program
{
    private static IConfiguration _config;

    private static void Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();

        try
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();

            var host = CreateHostBuilder(args).Build();

            Scoping(host.Services, "default");

            host.RunAsync();
        }
        catch (Exception e)
        {
            logger.Error(e);
            throw;
        }
    }

    private static void Scoping(IServiceProvider services, string @default)
    {
        var serviceScope = services.CreateAsyncScope();
        var serviceScopeServiceProvider = serviceScope.ServiceProvider;
        var app = serviceScopeServiceProvider.GetRequiredService<App>();
        app.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
                services
                    .AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                        loggingBuilder.AddNLog(_config);
                    })
                    .AddScoped<App>()
                    .AddScoped<IService, Service>());
}
