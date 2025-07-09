using backend.Application.DTOs.Category;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface ICategoryService 
    {
        Task<Response<GetCategoryResponseDto>> GetCategoryAsync(int productId);
        Task<Response<NoContent>> AddCategoryAsync(CreateCategoryRequestDto product);
        Task<Response<NoContent>> UpdateCategoryAsync(UpdateCategoryRequestDto newProduct);
        Task<Response<NoContent>> DeleteCategoryAsync(int productId);
        Task<Response<IEnumerable<GetCategoryResponseDto>>> GetCategoryAllAsync();
    }
}
