using AutoMapper;
using Azure.Core;
using backend.Application.DTOs.Auth;
using backend.Application.DTOs.Token;
using backend.Application.DTOs.UserRole;
using backend.Application.Services;
using backend.Domain.Entities;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly IService<User> _userService;
        private readonly IService<RefreshToken> _refreshTokenService;
        private readonly IUserRoleService _roleService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthService(IMapper mapper, IService<User> userService, IUserRoleService roleService, ITokenService tokenService, IService<RefreshToken> refreshTokenService)
        {
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userService.GetFirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash))
                return Response<LoginResponseDto>.Fail("Giriş başarısız", HttpStatusCode.Unauthorized);

            var role = await _roleService.GetUserRoleAsync(user.Id);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role.Data.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var accessTokenDto = _tokenService.GenerateToken(claims);

            var refreshTokenDto = _tokenService.GenerateRefreshToken();

            var existingToken = await _refreshTokenService.GetFirstOrDefaultAsync(x => x.UserId == user.Id);

            if (existingToken != null)
            {
                existingToken.Token = refreshTokenDto.RefreshToken;
                existingToken.ExpirationDate = refreshTokenDto.RefreshTokenExpTime;
                await _refreshTokenService.UpdateAsync(existingToken);
            }
            else
            {
                var refreshToken = new RefreshToken
                {
                    Token = refreshTokenDto.RefreshToken,
                    ExpirationDate = refreshTokenDto.RefreshTokenExpTime,
                    UserId = user.Id
                };
                await _refreshTokenService.AddAsync(refreshToken);
            }

            var userResponse = _mapper.Map<User, UserResponseDto>(user);
            var loginResponse = new LoginResponseDto
            {
                UserInfo = userResponse,
                AccessToken = accessTokenDto,
                RefreshToken = refreshTokenDto
            };

            return Response<LoginResponseDto>.Success(loginResponse, HttpStatusCode.OK, "Giriş başarılı");
        }

        public async Task<Response<NoContent>> RegisterAsync(RegisterRequestDto registerRequest)
        {
            if (await _userService.GetFirstOrDefaultAsync(u => u.Email == registerRequest.Email) != null)
                return Response<NoContent>.Fail("Zaten kayıtlı hesap!", HttpStatusCode.BadRequest);

            var user = _mapper.Map<RegisterRequestDto, User>(registerRequest, opt =>
            {
                opt.Items["PasswordHash"] = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            });

            await _userService.AddAsync(user);
            await _roleService.AddUserRoleAsync(new CreateUserRoleRequest { UserId = user.Id, Role = "User" });
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kayıt başarılı");
        }

        public async Task<bool> IsEmailExist(string email)
        {
            if (await _userService.GetFirstOrDefaultAsync(p => p.Email == email) != null)
                return true;
            return false;
        }

        public async Task<Response<TokensResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto tokenRequest)
        {
            var existingToken = await _refreshTokenService.Query().Include(x => x.User).FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
            if (existingToken == null || existingToken?.ExpirationDate < DateTime.UtcNow)
            {
                return Response<TokensResponseDto>.Fail("Geçersiz veya tarihi geçmiş token!", HttpStatusCode.Unauthorized);
            }
            var role = await _roleService.GetUserRoleAsync(existingToken.User.Id);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, existingToken.User.Id.ToString()),
                new Claim(ClaimTypes.Name, existingToken.User.Username),
                new Claim(ClaimTypes.Email, existingToken.User.Email),
                new Claim(ClaimTypes.Role, role.Data.Role),
                new Claim(ClaimTypes.Email, existingToken.User.Email)
            };
            var newGeneratedTokenDto = _tokenService.GenerateToken(claims);
            var newGeneratedRefreshTokenDto = _tokenService.GenerateRefreshToken();


            existingToken.Token = newGeneratedRefreshTokenDto.RefreshToken;
            existingToken.ExpirationDate = newGeneratedRefreshTokenDto.RefreshTokenExpTime;
            await _refreshTokenService.UpdateAsync(existingToken);
            var tokensResponse = new TokensResponseDto
            {
                AccessToken = newGeneratedTokenDto,
                RefreshToken = newGeneratedRefreshTokenDto
            };

            return Response<TokensResponseDto>.Success(tokensResponse, HttpStatusCode.OK, "Token güncellendi");
        }
    }
}
