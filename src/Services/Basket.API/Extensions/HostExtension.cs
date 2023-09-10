namespace Basket.API.Extensions
{
    public static class HostExtension
    {
        public static void AddAppConfiguration(this ConfigureHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile(path:"appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile(path:$"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange:true)
                        .AddEnvironmentVariables();
            });
        }
    }
}
