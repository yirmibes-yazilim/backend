using backend.Application.DTOs.Auth;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly IService<User> _userService;

        public UserService(IService<User> userService)
        {
            _userService = userService;
        }

        public async Task<Response<List<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.Query()
                .Include(u => u.Roles)
                .ToListAsync();

            if (users == null || !users.Any())
            {
                return Response<List<UserResponseDto>>.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);
            }

            var userDtos = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsEmailConfirmed = u.IsEmailConfirmed,
                Roles = u.Roles.Select(ur => ur.Role).ToList()
            }).ToList();

            return Response<List<UserResponseDto>>.Success(userDtos, HttpStatusCode.OK, "Kullanıcılar başarıyla getirildi");
        }


        public async Task<Response<User>> FindByEmailAsync(string email)
        {
            var user = await _userService.GetFirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Response<User>.Fail("Kullanıcı bulunamadı", System.Net.HttpStatusCode.NotFound);
            }
            return Response<User>.Success(user, System.Net.HttpStatusCode.OK, "Kullanıcı bulundu");
        }
    }
}
