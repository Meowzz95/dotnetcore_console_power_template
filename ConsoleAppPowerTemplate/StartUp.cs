using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleAppPowerTemplate
{
    public class StartUp
    {
        private ILogger<StartUp> _logger;
        private IConfiguration _config;

        public StartUp(ILogger<StartUp> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void Run()
        {
            _logger.LogDebug("Hello world!");
        }
    }
}