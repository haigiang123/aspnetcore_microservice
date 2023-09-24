using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed 
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;

        public OrderContextSeed(ILogger logger, OrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task TrySeedAsync(OrderContext context)
        {
            _logger.Information($"Try Seed Data for {context.Database.ProviderName}");
            if(!context.Orders.Any())
            {
                await context.Orders.AddRangeAsync(new Order
                {
                    FirstName = "customer1",
                    LastName = "customer",
                    EmailAddress = "customer1@gmail.com",
                    UserName = "username1",
                    TotalPrice = Convert.ToDecimal(456.23),
                    ShippingAddress = "Canada",
                    InvoiceAddress = "Toronto",
                    Status = 0,
                });
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync(_context);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occured while seeding the database.");
                throw;
            }
        }

        public async Task InitializeAsync()
        {
            try
            {
                if(_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occured while initializing database.");
                throw;   
            }
        }
    }
}
