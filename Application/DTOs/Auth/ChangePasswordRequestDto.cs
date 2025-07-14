namespace backend.Application.DTOs.Auth
{
    public class ChangePasswordRequestDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
