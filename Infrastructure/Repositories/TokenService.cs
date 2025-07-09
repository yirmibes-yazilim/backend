using backend.Application.DTOs.Token;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Infrastructure.Repositories
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AccessTokenResponseDto GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int accessTokenMinutes = _configuration.GetValue<int>("Jwt:AccessTokenMinutes");
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(accessTokenMinutes),
                signingCredentials: creds);

            var accessToken = new AccessTokenResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                AccessTokenExpTime = DateTime.Now.AddMinutes(accessTokenMinutes)
            };
            return accessToken;
        }

        public RefreshTokenResponseDto GenerateRefreshToken()
        {
            Guid myuuid = Guid.NewGuid();
            var myuuidAsString = myuuid.ToString();
            int refreshTokenDays = _configuration.GetValue<int>("Jwt:RefreshTokenDays");
            var refreshToken = new RefreshTokenResponseDto
            {
                RefreshToken = myuuidAsString,
                RefreshTokenExpTime = DateTime.Now.AddDays(refreshTokenDays)
            };
            return refreshToken;
        }
    }
}

