namespace backend.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }   
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public Category Category { get; set; }
        public List<CardItem> CardItems { get; set; }
    }
}
