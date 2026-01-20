using InventoryManagement.API.Entities;

namespace InventoryManagement.API.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string password);
    }

}
