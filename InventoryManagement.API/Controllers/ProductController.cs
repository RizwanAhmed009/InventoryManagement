using InventoryManagement.API.DTOs;
using InventoryManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;

        public ProductController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await service.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("Get-Product-ById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await service.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product not found.");
            return Ok(product);
        }

        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(ProductDto dto)
        {
            var product = await service.AddProductAsync(dto);
            if (product == null) return BadRequest("Category not found or invalid.");
            return Ok(product);
        }

        [HttpPut("Update-Product/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
        {
            var product = await service.UpdateProductAsync(id, dto);
            if (product == null) return NotFound("Product or Category not found.");
            return Ok(product);
        }

        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await service.DeleteProductAsync(id);
            if (!success) return NotFound("Product not found.");
            return Ok("Product deleted successfully.");
        }
    }
}
