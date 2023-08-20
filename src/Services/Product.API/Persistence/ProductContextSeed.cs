using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
        {
            if (!productContext.Products.Any())
            {
                productContext.AddRange(GetCatalogProducts());
                await productContext.SaveChangesAsync();
                
                logger.Information(messageTemplate: "Seed data for ProductDB associated with context {DBContextName}",
                    propertyValue: nameof(ProductContext));
            }
        }

        public static IEnumerable<CatalogProduct> GetCatalogProducts()
        {
            return new List<CatalogProduct>()
            {
                new CatalogProduct
                {
                    No = "Lotus 1",
                    Name = "Name 1",
                    Summary = "Summary 1",
                    Description = "Description 1",
                    Price = (decimal)177739.12
                },
                new CatalogProduct
                {
                    No = "Lotus 2",
                    Name = "Name 2",
                    Summary = "Summary 2",
                    Description = "Description 2",
                    Price = (decimal)187739.12
                }
            };
        } 
    }
}
