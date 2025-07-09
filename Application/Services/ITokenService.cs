using backend.Application.DTOs.Token;
using backend.Domain.Entities;
using System.Security.Claims;

namespace backend.Application.Services
{
    public interface ITokenService
    {
        public AccessTokenResponseDto GenerateToken(IEnumerable<Claim> claim);
        public RefreshTokenResponseDto GenerateRefreshToken();
    }
}
