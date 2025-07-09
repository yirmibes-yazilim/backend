namespace backend.Application.DTOs.Token
{
    public class TokensResponseDto
    {
        public AccessTokenResponseDto AccessToken { get; set; }
        public RefreshTokenResponseDto RefreshToken { get; set; }
    }
}
