using backend.Application.DTOs.Auth;
using backend.Domain.Entities;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IUserService
    {
        Task<Response<List<UserResponseDto>>> GetAllUsers();
        Task<Response<User>> FindByEmailAsync(string email);
    }
}
