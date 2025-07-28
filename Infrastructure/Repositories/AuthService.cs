using AutoMapper;
using backend.Application.DTOs.Auth;
using backend.Application.DTOs.Token;
using backend.Application.DTOs.UserRole;
using backend.Application.Services;
using backend.Domain.Entities;
using backend.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IMailService _mailService;
        private readonly IService<EmailVerificationToken> _verifyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IMapper mapper, IService<User> userService, IUserRoleService roleService, ITokenService tokenService, IService<RefreshToken> refreshTokenService, IMailService mailService, IService<EmailVerificationToken> verifyService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _mailService = mailService;
            _verifyService = verifyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userService.GetFirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash))
                return Response<LoginResponseDto>.Fail("Giriş başarısız", HttpStatusCode.Unauthorized);

            var rolesResult = await _roleService.GetUserRoleAsync(user.Id);
            var roles = rolesResult.Data ?? new List<GetUserRoleResponse>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Role)));

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
            userResponse.Roles = roles.Select(r => r.Role).ToList(); // ← İstersen frontend'e gönder

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
            var existingToken = await _refreshTokenService.Query()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (existingToken == null || existingToken.ExpirationDate < DateTime.UtcNow)
            {
                return Response<TokensResponseDto>.Fail("Geçersiz veya tarihi geçmiş token!", HttpStatusCode.Unauthorized);
            }

            var rolesResult = await _roleService.GetUserRoleAsync(existingToken.User.Id);
            var roles = rolesResult.Data ?? new List<GetUserRoleResponse>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingToken.User.Id.ToString()),
                new Claim(ClaimTypes.Name, existingToken.User.Username),
                new Claim(ClaimTypes.Email, existingToken.User.Email)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Role)));

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

        public async Task<Response<NoContent>> EmailSendConfirmTokenAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userService.GetFirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var confirmToken = _tokenService.GenerateEmailConfirmToken(user.Id);
                var token = new EmailVerificationToken
                {
                    UserId = user.Id,
                    Code = confirmToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10)
                };
                await _verifyService.AddAsync(token);

                await _mailService.SendEmailAsync(user.Email, "Verify Email", confirmToken);
                return Response<NoContent>.Success(HttpStatusCode.OK, "Email doğrulama kodu gönderildi");
            }
            else               
                return Response<NoContent>.Fail("Kayıtlı mail bulunamadı", HttpStatusCode.NotFound); 
        }

        public async Task<Response<NoContent>> VerifyEmailConfirmTokenAsync(string confirmationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var token = await _verifyService
                            .GetFirstOrDefaultAsync(t =>
                                t.UserId == userId &&
                                t.Code == confirmationToken &&
                                t.IsUsed == false &&
                                t.ExpiresAt > DateTime.UtcNow);

            if (token is null)
                return Response<NoContent>.Fail("Geçersiz veya süresi dolmuş token", HttpStatusCode.BadRequest);

            token.IsUsed = true;
            var account = await _userService.GetByIdAsync(userId);
            account.IsEmailConfirmed = true;

            await _userService.UpdateAsync(account);
            await _verifyService.DeleteAsync(token.Id);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Email doğrulandı");
        }

        public async Task<Response<NoContent>> ChangePassword(ChangePasswordRequestDto changePasswordRequest)
        {
            var user = await _userService.GetByIdAsync(changePasswordRequest.UserId);
            if (user == null)
                return Response<NoContent>.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);
            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.PasswordHash))
                return Response<NoContent>.Fail("Eski şifre yanlış", HttpStatusCode.BadRequest);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword);
            await _userService.UpdateAsync(user);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Şifre değiştirildi");
        }
    }
}
