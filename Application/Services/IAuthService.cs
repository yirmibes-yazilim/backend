using backend.Application.DTOs.Auth;
using backend.Application.DTOs.Token;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IAuthService
    {
        Task<Response<NoContent>> RegisterAsync(RegisterRequestDto registerRequest);
        Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> IsEmailExist(string email);
        Task<Response<TokensResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto tokenRequest);
        Task<Response<NoContent>> EmailSendConfirmTokenAsync(int id);
        Task<Response<NoContent>> VerifyEmailConfirmTokenAsync(int userId, string confirmationToken);
        Task<Response<NoContent>> ChangePassword(ChangePasswordRequestDto changePasswordRequest);
    }
}

