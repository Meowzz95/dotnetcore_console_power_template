using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleAppPowerTemplate
{
    class Program
    {
        private static ILogger<Program> _logger;
        private static bool _envProvided;
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<StartUp>().Run();
            serviceProvider.Dispose();
            
        }

        static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = GetConfiguration();
            serviceCollection.AddLogging(config=>config.AddConsole().AddConfiguration(GetConfiguration().GetSection("Logging")));
            _logger = serviceCollection.BuildServiceProvider().GetService<ILogger<Program>>();
            if(!_envProvided) _logger.LogWarning("Environment is not provided, using DEV.");
            serviceCollection.AddSingleton<StartUp>();
            serviceCollection.AddSingleton(configuration);
            
            // If EF Core is used
            // You need to add connector lib in NuGet
            // serviceCollection.AddDbContext<AppDbContext>(opt =>
            // {
            //     opt.UseNpgsql(configuration.GetSection("Db").GetValue<string>("ConnStr"));
            //     opt.UseLazyLoadingProxies();
            //     
            // });
        }

        static IConfiguration GetConfiguration()
        {
            string env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            if (String.IsNullOrWhiteSpace(env))
            {
                env = "DEV";
            }
            else
            {
                _envProvided = true;
            }
               
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile($"appsettings.{env}.json",false,true);
            return builder.Build();
        }
    }
}