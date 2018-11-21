using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TAC.Web.IntegrationTests
{
    public class FakeWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(new ServiceDescriptor(typeof(VehicleAvailabilityCheckerService), typeof(IHostedService), ServiceLifetime.Singleton));
                services.Remove(new ServiceDescriptor(typeof(VehicleStatusUpdateMachineryService), typeof(IHostedService), ServiceLifetime.Singleton));
            });
        }
    }
}
