using backend.Application.DTOs.CardItem;
using backend.Application.DTOs.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface ICardItemService
    {
        Task<Response<GetCardItemResponseDto>> GetCardItemByIdAsync(int cardItemId);
        Task<Response<NoContent>> AddCardItemAsync(CreateCardItemRequestDto cardItem);
        Task<Response<NoContent>> UpdateCardItemsQuantityAsync(int cardItemId,int newQuantity);
        Task<Response<NoContent>> DeleteCardItemAsync(int cardItemId);
        Task<Response<IEnumerable<GetCardItemResponseDto>>> GetCardItemsAllByUserIdAsync();
        Task<Response<NoContent>> ClearCardItemsAllByUserIdAsync();
    }
}
