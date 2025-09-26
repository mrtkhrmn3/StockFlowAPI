using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockFlowAPI.BusinessLayer.DTOs;
using StockFlowAPI.BusinessLayer.Interfaces;

namespace StockFlowAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (!result) return BadRequest("Registration failed. Check username or password confirmation.");

            return Ok("User registered successfully.");
        }

 [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _userService.LoginAsync(dto);
            if (result == null)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre");

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDTO dto)
        {
            var result = await _userService.RefreshTokenAsync(dto);
            if (result == null)
                return Unauthorized("Geçersiz veya süresi dolmuş refresh token");

            return Ok(result);
        }
    }
}

