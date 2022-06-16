using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Saubian.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string APP_CONFIG_ENDPOINT_KEY = "ConfigEndpoint";

        public static IConfigurationRoot BuildAppConfigRoot(this IServiceCollection services)
        {
            var providers = new List<IConfigurationProvider>();
            var appConfigEndpoint = Environment.GetEnvironmentVariable(APP_CONFIG_ENDPOINT_KEY);

            //if (string.IsNullOrWhiteSpace(appConfigEndpoint))
            //    throw new ArgumentException($"Parameter {APP_CONFIG_ENDPOINT_KEY} cannot be null");

            // Add local.settings.json App Config
            var globalConfig = BuildLocalAppConfig(services);
            providers.AddRange(globalConfig.Providers);

            return new ConfigurationRoot(providers);
        }
        private static IConfigurationRoot BuildLocalAppConfig(IServiceCollection services)
        {
            var executionContextOptions = services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>()
                .Value;

            var basePath = GetApplicationBasePath(executionContextOptions);

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: false)
                .Build();
        }

        private static string GetApplicationBasePath(ExecutionContextOptions executionContext)
        {
            var basePath = executionContext.AppDirectory;

            if (!string.IsNullOrWhiteSpace(basePath))
                return basePath;

            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
