using InventoryManagement.API.Data;
using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;
using InventoryManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext context;

        public CategoryService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await context.Categories.Where(c=>c.IsActive).ToListAsync();
            return categories.Select(c => new CategoryDto { 
            Id=c.Id,
            Name=c.Name,
            
            });
          
        }
        public async Task<CategoryDto> AddCategoryAsync(CategoryDto dto)
        {
            var category = new Category { 
            Name = dto.Name,
            IsActive = true
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
        public async Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto dto)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (category == null) return null;

            category.Name = dto.Name; 
            await context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (category == null) return false;

            category.IsActive = false;
            await context.SaveChangesAsync();
            return true;
        }






    }
}
