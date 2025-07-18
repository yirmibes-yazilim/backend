namespace backend.Application.DTOs.Auth
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}