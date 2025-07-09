using System.ComponentModel.DataAnnotations;

namespace backend.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}