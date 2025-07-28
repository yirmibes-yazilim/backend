using backend.Application.DTOs.CardItem;
using backend.Application.DTOs.FavoriteProduct;
using backend.Application.DTOs.Filter;
using backend.Application.DTOs.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IFavoriteProductService
    {
        Task<Response<NoContent>> AddFavoriteProductAsync(CreateFavoriteProductRequestDto req);
        Task<Response<IEnumerable<GetProductResponseDto>>> GetUserFavoriteProductAll();
        Task<Response<NoContent>> RemoveFavoriteProductAsync(RemoveFavoriteProductRequestDto req);
    }
}
