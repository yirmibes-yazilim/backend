using backend.Application.Services;
using backend.Domain.Entities;
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
