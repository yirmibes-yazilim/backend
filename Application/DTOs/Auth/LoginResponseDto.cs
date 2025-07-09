using backend.Application.DTOs.Token;

namespace backend.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public UserResponseDto UserInfo { get; set; }
        public AccessTokenResponseDto AccessToken { get; set; }
        public RefreshTokenResponseDto RefreshToken { get; set; }
    }
}
