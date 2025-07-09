namespace backend.Application.DTOs.Token
{
    public class AccessTokenResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpTime { get; set; }
    }
}
