namespace backend.Application.DTOs.Token
{
    public class RefreshTokenResponseDto
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpTime { get; set; }
    }
}
