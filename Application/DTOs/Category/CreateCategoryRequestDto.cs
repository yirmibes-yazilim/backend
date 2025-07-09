namespace backend.Application.DTOs.Category
{
    public class CreateCategoryRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
