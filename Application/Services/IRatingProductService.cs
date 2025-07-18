using backend.Application.DTOs.RatingProduct;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IRatingProductService
    {
        public Task<Response<NoContent>> AddRatingAsync(CreateRatingProductRequestDto createRatingProductRequestDto);
        public Task<Response<GetRatingProductResponseDto>> GetUserRatingAsync(int productId, int userId);
        public Task<Response<IEnumerable<GetRatingProductResponseDto>>> GetRatingsByProductAsync(int productId);
        public Task<Response<IEnumerable<GetRatingProductResponseDto>>> GetRatingsByUserAsync(int userId);
        public Task<Response<NoContent>> DeleteRatingAsync(int productId, int userId);
    }
}
