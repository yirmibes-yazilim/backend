using backend.Application.DTOs.Filter;
using backend.Application.DTOs.Product;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IProductService 
    {
        Task<Response<GetProductResponseDto>> GetProductAsync(int productId);
        Task<Response<NoContent>> AddProductAsync(CreateProductRequestDto product);
        Task<Response<NoContent>> UpdateProductAsync(UpdateProductRequestDto newProduct);
        Task<Response<NoContent>> DeleteProductAsync(int productId);
        Task<Response<IEnumerable<GetProductResponseDto>>> GetProductAllAsync(ProductFilter filter);
        Task<bool> IsProductNameExist(string name);
        Task<Response<IEnumerable<GetProductResponseDto>>> GetProductByCategoryAsync(int categoryId);
        Task<Response<IEnumerable<GetProductResponseDto>>> GetProductByNameAsync(string name);
        Task<Response<IEnumerable<GetProductResponseDto>>> GetProductByPriceRange(int minPrice, int maxPrice);
    }
}
