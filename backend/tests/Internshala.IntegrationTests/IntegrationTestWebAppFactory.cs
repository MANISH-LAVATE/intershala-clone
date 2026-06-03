using Internshala.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Internshala.IntegrationTests;

/// <summary>
/// Shared WebApplicationFactory that replaces SQL Server with an in-memory database.
/// Each test class receives a fresh database per factory instance.
/// </summary>
public sealed class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "test-secret-key-for-integration-tests-32c",
                ["Jwt:Issuer"] = "InternshalaTest",
                ["Jwt:Audience"] = "InternshalaTestAudience",
                ["Jwt:ExpiryMinutes"] = "15",
                ["ConnectionStrings:DefaultConnection"] = "Server=noop;Database=InternshalaTest;"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove existing EF Core SQL Server registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor is not null)
                services.Remove(descriptor);

            // Replace with in-memory database (unique per factory instance)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));
        });
    }
}
