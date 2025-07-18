
using backend.Application.DTOs.Auth;
using backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);

            if (!result.IsSuccessful)
                return Unauthorized(result.Message);

            if (!result.Data.UserInfo.Roles.Contains("Admin"))
                return Forbid("Bu giriş sadece Admin kullanıcılar içindir.");

            return Ok(result);
        }
    }
}
