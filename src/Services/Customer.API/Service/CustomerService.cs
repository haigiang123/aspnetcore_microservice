using Customer.API.Repositories.Interface;
using Customer.API.Service.Interface;

namespace Customer.API.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customer;
        public CustomerService(ICustomerRepository customer) 
        {
            _customer = customer;
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username)
        {
            return  Results.Ok(await _customer.GetCustomerByUserNameAsync(username));
        }

        public async Task<IResult> GetCustomersAsync()
        {
            return Results.Ok(await _customer.GetCustomersAsync());
        }
    }
}
