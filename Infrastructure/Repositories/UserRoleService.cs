using AutoMapper;
using backend.Application.DTOs.UserRole;
using backend.Application.Services;
using backend.Domain.Entities;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IMapper _mapper;
        private readonly IService<UserRole> _service;
        public UserRoleService(IMapper mapper, IService<UserRole> service = null)
        {
            _mapper = mapper;
            _service = service;
        }
        public async Task<Response<NoContent>> AddUserRoleAsync(CreateUserRoleRequest userRoleRequest)
        {
            var userRole = _mapper.Map<CreateUserRoleRequest, UserRole>(userRoleRequest);
            await _service.AddAsync(userRole);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Rol Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteUserRoleAsync(int userId)
        {
            var userRole = await _service.GetFirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole == null)
            {
                return Response<NoContent>.Fail("Silme Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                await _service.DeleteAsync(userRole.Id);
                return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
            }
        }

        public async Task<Response<List<GetUserRoleResponse>>> GetUserRoleAllAsync()
        {
            var userRoles = await _service.GetAllAsync();
            var response = _mapper.Map<List<UserRole>, List<GetUserRoleResponse>>((List<UserRole>)userRoles);
            return Response<List<GetUserRoleResponse>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<List<GetUserRoleResponse>>> GetUserRoleAsync(int userId)
        {
            var userRoles = await _service.Query().Where(x => x.UserId == userId).ToListAsync();

            if (userRoles == null)
            {
                return Response<List<GetUserRoleResponse>>.Fail("Kullanıcı Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<List<UserRole>, List<GetUserRoleResponse>>(userRoles);
                return Response<List<GetUserRoleResponse>>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateUserRoleAsync(UpdateUserRoleRequest userRoleRequest)
        {
            var userRole = await _service.GetByIdAsync(userRoleRequest.Id);
            if (userRole == null)
            {
                return Response<NoContent>.Fail("Güncelleme Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                userRole.Role = userRoleRequest.Role;

                await _service.UpdateAsync(userRole);
                return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme Başarılı!");
            }
        }
    }
}
