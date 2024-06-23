using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

using Microsoft.EntityFrameworkCore;
using Api.Data;

namespace Integration.Tests.Shared;

public class SharedApiTestFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

    // Configure Application Host - Configuration
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }

    // Configure Web Host - Services etc...
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApiContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<ApiContext>(options => { options.UseSqlServer(_msSqlContainer.GetConnectionString()); });
        });
    }


    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public ApiContext CreateContext()
        => new(
            new DbContextOptionsBuilder<ApiContext>()
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .Options);

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}