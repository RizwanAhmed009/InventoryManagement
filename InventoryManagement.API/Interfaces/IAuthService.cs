using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;

namespace InventoryManagement.API.Interfaces
{
    public interface IAuthService
    {
        Task<RefreshTokenResponseDto?> LoginAsync(LoginRequestDto request);

        Task<bool> RegisterAsync(RegisterRequestDto request);
        Task<RefreshTokenResponseDto?> RefreshTokenAsync(string refreshToken);


    }
}
