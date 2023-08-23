using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        #region
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var productList = await _productRepository.GetProducts();
            var result = _mapper.Map<IEnumerable<ProductDto>>(productList);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct([Required]long id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductDto productDto)
        {
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _productRepository.CreateAsync(product);
            await _productRepository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProduct([Required]long id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            var updateProduct = _mapper.Map(productDto, product);
            await _productRepository.UpdateProduct(updateProduct);
            await _productRepository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);

        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct([Required]long id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProduct(id);
            await _productRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("get-product-by-no/{productNo}")]
        public async Task<IActionResult> GetProductByNo([Required]string productNo)
        {
            var product = await _productRepository.GetProductByNo(productNo);
            if(product == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ProductDto>(product);
            return Ok(product);
        }

        #endregion

    }
}
