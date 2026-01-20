using InventoryManagement.API.Data;
using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;
using InventoryManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext context;

        public ProductService(AppDbContext context)
        {
            this.context = context;
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                TotalPrice = product.TotalPrice,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                IsActive = product.IsActive
            };
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToListAsync();

            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return null;
            return MapToDto(product);
        }

        public async Task<ProductDto?> AddProductAsync(ProductDto dto)
        {
            var category = await context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.IsActive);
            if (category == null) return null;

            var existingProduct = await context.Products
                .FirstOrDefaultAsync(p =>
                    p.Name.ToLower() == dto.Name.ToLower() &&
                    p.CategoryId == dto.CategoryId &&
                    p.IsActive);

            if (existingProduct != null)
            {
                existingProduct.Quantity += dto.Quantity;

                existingProduct.TotalPrice = existingProduct.Price * existingProduct.Quantity;

                await context.SaveChangesAsync();

                return MapToDto(existingProduct);
            }

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                TotalPrice = dto.Price * dto.Quantity,
                CategoryId = dto.CategoryId,
                IsActive = true
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            product.Category = category;
            return MapToDto(product);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, ProductDto dto)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product == null) return null;

            if (dto.CategoryId != product.CategoryId)
                return null;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.TotalPrice = dto.Price * dto.Quantity;

            await context.SaveChangesAsync();

            product.Category = await context.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);

            return MapToDto(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product == null) return false;

            product.IsActive = false;
            await context.SaveChangesAsync();

            return true;
        }
    }
}
