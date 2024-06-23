using Api.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApiContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultContext")));

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApiContext>();
            context.Database.EnsureCreated();
        }

        app.MapControllers();

        return app;
    }
}