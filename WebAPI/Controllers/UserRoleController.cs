using backend.Application.DTOs.UserRole;
using backend.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        [HttpGet("getUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRolesByUserId(int userId)
        {
            return Ok(await _userRoleService.GetUserRoleAsync(userId));
        }
        [HttpPost("addUserRole")]
        public async Task<IActionResult> AddUserRoleAsync(CreateUserRoleRequest createUserRoleRequest)
        {
            return Ok(await _userRoleService.AddUserRoleAsync(createUserRoleRequest));
        }
        [HttpPut("updateUserRole")]
        public async Task<IActionResult> UpdateUserRoleAsync(UpdateUserRoleRequest updateUserRoleRequest)
        {
            return Ok(await _userRoleService.UpdateUserRoleAsync(updateUserRoleRequest));
        }
        [HttpDelete("removeUserRole")]
        public async Task<IActionResult> RemoveUserRoleAsync(int userId)
        {
            return Ok(await _userRoleService.DeleteUserRoleAsync(userId));
        }
        [HttpGet("getAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            return Ok(await _userRoleService.GetUserRoleAllAsync());
        }
    }
}
