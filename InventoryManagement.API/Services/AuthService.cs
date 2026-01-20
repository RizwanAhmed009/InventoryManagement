using InventoryManagement.API.Data;
using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;
using InventoryManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace InventoryManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext context;
        private readonly IPasswordService passwordService;
        private readonly IJwtService jwtService;

        public AuthService(
            AppDbContext context,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            this.context = context;
            this.passwordService = passwordService;
            this.jwtService = jwtService;
        }

        // ===================== LOGIN =====================
        public async Task<RefreshTokenResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return null;

            var isValidPassword =
                passwordService.VerifyPassword(user, request.Password);

            if (!isValidPassword)
                return null;

            var accessToken = jwtService.GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            await SaveRefreshToken(user.Id, refreshToken);

            return new RefreshTokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }



        // ===================== REGISTER =====================
        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            var exist = await context.Users.AnyAsync(u => u.Email == request.Email);
            if (exist) return false;

            var role = await context.Role.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (role == null) return false;

            var tempUser = new User();
            var hashedPassword =
                passwordService.HashPassword(tempUser, request.Password);

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = hashedPassword,
                RoleId = role.Id,
                IsActive = true
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return true;
        }

        // ===================== REFRESH TOKEN =====================
        public async Task<RefreshTokenResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await context.RefreshTokens
                .Include(rt => rt.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity == null)
                return null;

            if (tokenEntity.Expires < DateTime.UtcNow)
                return null;

            var newAccessToken =
                jwtService.GenerateToken(tokenEntity.User);

            var newRefreshToken = GenerateRefreshToken();

            tokenEntity.Token = newRefreshToken;
            tokenEntity.Expires = DateTime.UtcNow.AddDays(7);

            await context.SaveChangesAsync();

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        // ===================== PRIVATE METHODS =====================

        private string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        private async Task SaveRefreshToken(int userId, string refreshToken)
        {
            var token = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            await context.RefreshTokens.AddAsync(token);
            await context.SaveChangesAsync();
        }
    }
}
