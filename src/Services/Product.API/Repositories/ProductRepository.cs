using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories
{
    public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductContext>, IProductRepository
    {
        public ProductRepository(ProductContext productContext, IUnitOfWork<ProductContext> unitOfWork) : base(productContext, unitOfWork)
        {

        }

        public async Task CreateProduct(CatalogProduct product)
        {
            await CreateAsync(product);
        }

        public async Task DeleteProduct(long id)
        {
            var product = await GetProduct(id);
            await DeleteAsync(product);
        }

        public async Task<CatalogProduct> GetProduct(long id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<CatalogProduct>> GetProducts()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<CatalogProduct> GetProductByNo(string productNo)
        {
            return await FindByCondition(x => x.No.Equals(productNo)).FirstOrDefaultAsync();
        }

        public async Task UpdateProduct(CatalogProduct product)
        {
            await UpdateAsync(product);
        }
    }
}
