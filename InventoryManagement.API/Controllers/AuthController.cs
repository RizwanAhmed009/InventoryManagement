using InventoryManagement.API.DTOs;
using InventoryManagement.API.Interfaces;
using InventoryManagement.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request) {
            var token = await service.LoginAsync(request);
            if (token == null) 
                return NotFound();

            return Ok(new {token});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await service.RegisterAsync(request);
            if (!result) return BadRequest("Email already exists or registration failed.");
            return Ok("Registration successful");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await service.RefreshTokenAsync(request.RefreshToken);

            if (result == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }
    }
}
