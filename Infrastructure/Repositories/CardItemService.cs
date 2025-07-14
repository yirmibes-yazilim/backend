using AutoMapper;
using backend.Application.DTOs.CardItem;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class CardItemService : ICardItemService
    {
        private readonly IMapper _mapper;
        private readonly IService<CardItem> _service;
        public CardItemService(IMapper mapper, IService<CardItem> service = null)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Response<NoContent>> AddCardItemAsync(CreateCardItemRequestDto cardItem)
        {
            var newCardItem = _mapper.Map<CreateCardItemRequestDto, CardItem>(cardItem);
            await _service.AddAsync(newCardItem);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kart Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> ClearCardItemsAllByUserIdAsync(int userId)
        {
            var cardItems = await _service.Query()
                              .Where(c => c.UserId == userId)
                              .ExecuteDeleteAsync();
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kullanıcın sepeti temizlendi!");
        }

        public async Task<Response<NoContent>> DeleteCardItemAsync(int cardItemId)
        {
            var cardItem = await _service.GetByIdAsync(cardItemId);
            if (cardItem == null)
            {
                return Response<NoContent>.Fail("Silinecek ürün bulunamadı.", HttpStatusCode.BadRequest);
            }
            await _service.DeleteAsync(cardItemId);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
        }

        public async Task<Response<GetCardItemResponseDto>> GetCardItemByIdAsync(int cardItemId)
        {
            var cardItem = await _service.GetByIdAsync(cardItemId);
            if (cardItem == null)
            {
                return Response<GetCardItemResponseDto>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<CardItem, GetCardItemResponseDto>(cardItem);
                return Response<GetCardItemResponseDto>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<IEnumerable<GetCardItemResponseDto>>> GetCardItemsAllByUserIdAsync(int userId)
        {
            var cardItems = await _service.Query()
                  .Where(c => c.UserId == userId)
                  .ToListAsync();
            var response = _mapper.Map<IEnumerable<CardItem>, IEnumerable<GetCardItemResponseDto>>(cardItems);
            return Response<IEnumerable<GetCardItemResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<NoContent>> UpdateCardItemsQuantityAsync(int cardItemId, int newQuantity)
        {
            var cardItem = await _service.GetByIdAsync(cardItemId);
            if (cardItem == null)
            {
                return Response<NoContent>.Fail("Güncellenecek ürün bulunamadı.", HttpStatusCode.BadRequest);
            }
            cardItem.Quantity = newQuantity;
            await _service.UpdateAsync(cardItem);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme başarılı!");
        }
    }
}
