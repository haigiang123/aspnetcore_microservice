using Contracts.Common.Interfaces;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.API.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBaseAsync<CatalogProduct, long, ProductContext>
    {
        public Task<CatalogProduct> GetProduct(long id);
        public Task<IEnumerable<CatalogProduct>> GetProducts();
        public Task<CatalogProduct> GetProductByNo(string productNo);
        public Task CreateProduct(CatalogProduct product);
        public Task UpdateProduct(CatalogProduct product);
        public Task DeleteProduct(long id);
    }
}
