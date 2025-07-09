using backend.Application.DTOs.UserRole;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IUserRoleService
    {
        Task<Response<GetUserRoleResponse>> GetUserRoleAsync(int userId);
        Task<Response<List<GetUserRoleResponse>>> GetUserRoleAllAsync();
        Task<Response<NoContent>> AddUserRoleAsync(CreateUserRoleRequest userRoleRequest);
        Task<Response<NoContent>> UpdateUserRoleAsync(UpdateUserRoleRequest userRoleRequest);
        Task<Response<NoContent>> DeleteUserRoleAsync(int userId);
    }
}
