namespace backend.Domain.Entities
{
    public class RatingProduct : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; } = string.Empty;
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
