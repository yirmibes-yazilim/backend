namespace backend.Domain.Entities
{
    public class EmailVerificationToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; } 
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public User User { get; set; } 
    }
}
