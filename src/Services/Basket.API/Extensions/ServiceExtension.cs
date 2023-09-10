using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
            => services.AddScoped<IBasketRepository, BasketRepositoty>()
                        .AddTransient<ISerializeService, SerializeService>();

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection(key: "CacheSettings:ConnectionString").Value;
            if(string.IsNullOrEmpty(connection))
            {
                throw new ArgumentNullException("Redis connection string is not configurated");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connection;
            });
        }
    }
}
