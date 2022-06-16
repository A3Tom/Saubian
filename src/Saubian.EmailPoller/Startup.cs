using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Saubian.Application.Queries.GetEmailMessages;
using Saubian.Domain.Extensions;
using Saubian.Domain.Interfaces.Settings;
using Saubian.EmailPoller.Settings;

[assembly: FunctionsStartup(typeof(Saubian.EmailPoller.Startup))]
namespace Saubian.EmailPoller
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var appConfig = services.BuildAppConfigRoot();

            ConfigureSettings(services, appConfig);

            ConfigureMediator(services);

            ConfigureLogging(services, appConfig);
        }

        private void ConfigureSettings(IServiceCollection services, IConfigurationRoot appConfig)
        {
            var emailPollerSettings = appConfig
                .GetSection($"EmailPoller")
                .Get<EmailPollerSettings>();

            services.AddSingleton(emailPollerSettings);
            services.AddSingleton<IIMapAccountSettings>(emailPollerSettings);
        }

        public void ConfigureMediator(IServiceCollection services)
        {
            services.AddMediatR(typeof(GetEmailMessages));
        }

        private void ConfigureLogging(IServiceCollection services, IConfigurationRoot appConfig)
        {
            services.AddLogging(options =>
            {
                var instrumentationKey = appConfig.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");

                if (!string.IsNullOrWhiteSpace(instrumentationKey))
                {
                    options.AddApplicationInsights(instrumentationKey);
                }
            });
        }
    }
}