using InventoryManagement.API.Data;
using InventoryManagement.API.Entities;
using InventoryManagement.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.API.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<User> hasher;

        public PasswordService(PasswordHasher<User> _hasher)
        {
            hasher = _hasher;
        }

        public string HashPassword(User user, string password)
        {
            return hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string password)
        {
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;

        }
    }
}
