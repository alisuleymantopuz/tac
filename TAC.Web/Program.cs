using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace TAC.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Information))
                .UseStartup<Startup>();
            return builder;
        }
    }
}
