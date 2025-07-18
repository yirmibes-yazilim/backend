using backend.Application.DTOs.OrderItem;

namespace backend.Application.DTOs.Order
{
    public class GetOrderResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public int TotalAmount { get; set; }
        public bool Status { get; set; }
        public IEnumerable<GetOrderItemResponseDto> OrderItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
