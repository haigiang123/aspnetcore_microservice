using Infrastructure.Configurations;

namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services
            , IConfiguration configuration) 
        {
            var config = configuration.GetSection(nameof(SMTPEmailSetting)).Get<SMTPEmailSetting>();
            services.AddSingleton(config);

            return services;
        }
    }
}
