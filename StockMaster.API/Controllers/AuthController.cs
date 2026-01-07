using Microsoft.AspNetCore.Mvc;
using StockMaster.Core.DTOs;
using StockMaster.Service.Services;

namespace StockMaster.API.Controllers
{
    public class AuthController : CustomBaseController
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return CreateActionResult(await _authService.LoginAsync(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            return CreateActionResult(await _authService.RegisterAsync(registerDto));
        }
    }
}