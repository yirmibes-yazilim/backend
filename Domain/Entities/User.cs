namespace backend.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEmailConfirmed { get; set; } = false; 
        public List<UserRole> Roles { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<Address> Addresses { get; set; }
        public List<CardItem> CardItems { get; set; }
        public List<Order> Orders { get; set; }
        public List<FavoriteProduct> FavoriteProducts { get; set; }
        public List<EmailVerificationToken> EmailVerificationTokens { get; set; }
        public List<RatingProduct> RatingProducts { get; set; }

    }
}
