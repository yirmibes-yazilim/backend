namespace backend.Application.DTOs.CardItem
{
    public class CreateCardItemRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}