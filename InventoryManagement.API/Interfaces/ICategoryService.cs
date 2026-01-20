using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;

namespace InventoryManagement.API.Interfaces
{
    public interface ICategoryService
    {
        
            Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
            Task<CategoryDto?> GetCategoryByIdAsync(int id);

            Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
            Task<CategoryDto?> UpdateCategoryAsync(int id, CategoryDto dto);

            Task<bool> DeleteCategoryAsync(int id); 
        

    }
}
