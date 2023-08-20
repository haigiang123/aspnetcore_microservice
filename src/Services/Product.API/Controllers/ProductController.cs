using Microsoft.AspNetCore.Mvc;
using Product.API.Repositories.Interfaces;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var productList = await _productRepository.GetProducts();
            return Ok(productList);
        }
    }
}
