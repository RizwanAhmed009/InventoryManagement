using InventoryManagement.API.Entities;

namespace InventoryManagement.API.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
