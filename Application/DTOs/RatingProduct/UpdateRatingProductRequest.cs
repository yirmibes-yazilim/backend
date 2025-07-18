namespace backend.Application.DTOs.RatingProduct
{
    public class UpdateRatingProductRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
