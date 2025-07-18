namespace backend.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public int TotalAmount { get; set; }
        public bool Status { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
