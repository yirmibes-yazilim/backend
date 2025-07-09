using backend.Application.DTOs.Auth;
using backend.Application.DTOs.Token;
using backend.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService userService)
        {
            _authService = userService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<Response<NoContent>>> Register(RegisterRequestDto req)
        {
            return Ok(await _authService.RegisterAsync(req));
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<UserResponseDto>>> Login(LoginRequestDto req)
        {
            return Ok(await _authService.LoginAsync(req));
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<Response<NoContent>>> RefreshToken(RefreshTokenRequestDto tokenRequest)
        {
            return Ok(await _authService.RefreshTokenAsync(tokenRequest));
        }
    }
}
