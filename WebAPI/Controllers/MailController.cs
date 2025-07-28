using backend.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MailController : ControllerBase
    {
        private readonly IAuthService _authService;
        public MailController(IAuthService userService)
        {
            _authService = userService;
        }

        [HttpPost("email-send-confirm-token")]
        public async Task<ActionResult<Response<NoContent>>> EmailSendConfirmToken()
        {
            return Ok(await _authService.EmailSendConfirmTokenAsync());
        }
        [HttpPost("verify-email-confirm-token")]
        public async Task<ActionResult<Response<NoContent>>> VerifyEmailConfirmToken(string confirmationToken)
        {
            return Ok(await _authService.VerifyEmailConfirmTokenAsync(confirmationToken));
        }
    }
}
