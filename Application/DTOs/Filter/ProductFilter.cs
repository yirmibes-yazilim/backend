namespace backend.Application.DTOs.Filter
{
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? OrderByPriceAscending { get; set; }
    }
}
