using Customer.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    public static class CustomerController
    {
        public static void MapCustomersApi(this WebApplication app)
        {
            app.MapGet("/api/customers/{username}", async (string username, ICustomerService service) =>
            {
                var result = await service.GetCustomerByUsernameAsync(username);
                return result != null ? Results.Ok(result) : Results.NotFound(); 
            });
        }   
    }
}
