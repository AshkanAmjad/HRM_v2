using Serilog;

namespace HRM.Extensions
{
    public static class LogServiceExtensions
    {
        public static void ConfigureLogging(IHostBuilder hostBuilder)
        {
            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                               .Build();

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            hostBuilder.UseSerilog();
        }
    }
}
