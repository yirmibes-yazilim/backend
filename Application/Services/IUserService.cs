using backend.Domain.Entities;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IUserService
    {
        Task<Response<User>> FindByEmailAsync(string email);
    }
}
