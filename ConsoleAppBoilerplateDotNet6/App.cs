using Microsoft.Extensions.Configuration;
using NLog;

namespace ConsoleAppBoilerplateDotNet6;

internal class App
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public App(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = LogManager.GetCurrentClassLogger();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }
}