namespace backend.Application.DTOs.UserRole
{
    public class UpdateUserRoleRequest
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
    }
}
