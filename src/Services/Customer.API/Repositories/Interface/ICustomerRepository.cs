using Contracts.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interface
{
    public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer, int, CustomerContext>
    {
        Task<Entities.Customer> GetCustomerByUserNameAsync(string username);
        Task<IEnumerable<Entities.Customer>> GetCustomersAsync();
    }
}
