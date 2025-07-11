namespace backend.Application.DTOs.Product
{
    public class CreateProductRequestDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public IFormFile? Image { get; set; }
    }
}
