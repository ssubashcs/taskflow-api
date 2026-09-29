using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Api.Data;

namespace TaskFlow.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.Remove(
                    services.Single(
                        descriptor =>
                            descriptor.ServiceType == typeof(DbContextOptions<TaskFlowDbContext>)));

                services.AddDbContext<TaskFlowDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ApiTests");
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(configuration =>
            {
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                                                    {
                                                        ["Jwt:Key"] = "test-only-signing-key",
                                                        ["Jwt:Issuer"] = "TaskFlow.Api",
                                                        ["Jwt:Audience"] = "TaskFlow.ApiClient",
                                                        ["Jwt:ExpiresMinutes"] = "60"
                                                    });
            });

            return base.CreateHost(builder);
        }
    }
}
