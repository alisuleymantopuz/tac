using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TAC.Web.IntegrationTests
{
    public class FakeWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

        }
    }
}
