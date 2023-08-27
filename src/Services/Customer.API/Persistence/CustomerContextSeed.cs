using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Customer.API.Persistence
{
    public static class CustomerContextSeed
    {
        public static IHost SeedCustomerData(this IHost host)
        {
            var scope = host.Services.CreateScope();
            var customerContext = scope.ServiceProvider.GetRequiredService<CustomerContext>();
            customerContext.Database.MigrateAsync().GetAwaiter().GetResult();

            CreateCustomer(customerContext, "customer1", "customer1", "customer1", "customer1@gmail.com").GetAwaiter();
            CreateCustomer(customerContext, "customer2", "customer2", "customer2", "customer2@gmail.com").GetAwaiter();

            return host;
        }

        public static async Task CreateCustomer(CustomerContext context, string username, string firstname, string lastname, string emailaddress)
        {
            var customer = await context.Customers
                                .SingleOrDefaultAsync(x => x.UserName.Equals(username) ||
                                x.EmailAddess.Equals(emailaddress));

            if (customer == null)
            {
                await context.Customers.AddAsync(new Entities.Customer
                {
                    UserName = username,
                    FirstName = firstname,
                    LastName = lastname,
                    EmailAddess = emailaddress

                });
                await context.SaveChangesAsync();
            }
        }

    }
}
