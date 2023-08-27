using Serilog;
using Common.Logging;
using Customer.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Customer.API.Repositories.Interface;
using Customer.API.Repositories;
using Customer.API.Service.Interface;
using Customer.API.Service;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

Log.Information("Start Customer API up");

try
{
    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectstring = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(
        options => options.UseNpgsql(connectstring));

    builder.Services.AddScoped<ICustomerRepository, CustomerRepositoryAsync>()
                    .AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                    .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                    .AddScoped<ICustomerService, CustomerService>();
                    

    var app = builder.Build();

    // Syntax minimal api 
    app.MapGet("/api/customers", 
        async (ICustomerService customer) => await customer.GetCustomersAsync());
    app.MapGet("/api/customers/{username}",
        async (string username, ICustomerService customer) => await customer.GetCustomerByUsernameAsync(username));

    // Configure the HTTP request pipeline.

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.SeedCustomerData()
        .Run();

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
