using Serilog;
using Common.Logging;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Ordering.Application;
using Ordering.API.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Host.AddAppConfigurations(builder.Configuration);
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructureService(builder.Configuration);
    builder.Services.AddApplicationServices();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => 
            c.SwaggerEndpoint(url:"/swagger/v1/swagger.json", name:"Swagger Order API V1"));
    }

    using (var scope = app.Services.CreateAsyncScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
        await orderContextSeed.InitializeAsync();
        await orderContextSeed.SeedAsync();
    }

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    if (ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "Unhandled Exception");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}
