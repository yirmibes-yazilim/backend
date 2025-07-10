using AutoMapper;
using backend.Application.DTOs.Order;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IService<CardItem> _cardItemService;
        private readonly IService<Order> _orderService;
        private readonly IService<Address> _addressService;

        public OrderService(IMapper mapper, IService<CardItem> cardItemService, IService<Order> orderService, IService<Address> addressService)
        {
            _mapper = mapper;
            _cardItemService = cardItemService;
            _orderService = orderService;
            _addressService = addressService;
        }

        public async Task<Response<GetOrderResponseDto>> ConfirmCardAsync(int userId)
        {
            var cardItems = await _cardItemService.Query()
                .Where(x => x.UserId == userId)
                .Include(p => p.Product).ToListAsync();

            var defaultAddress = await _addressService.Query()
                .Where(x => x.UserId == userId && x.IsDefault)
                .FirstOrDefaultAsync();

            if (cardItems == null || cardItems.Count == 0)
                return Response<GetOrderResponseDto>.Fail("Sepet Boş.", HttpStatusCode.BadRequest);

            if(defaultAddress == null)
                return Response<GetOrderResponseDto>.Fail("Adres seçilmedi.", HttpStatusCode.BadRequest);

            var order = new Order
            {
                UserId = userId,
                AddressId = defaultAddress.Id,
                TotalAmount = cardItems.Sum(x => x.Product.Price * x.Quantity),
                Status = true,
                OrderItems = cardItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Price = x.Product.Price,
                    Quantity = x.Quantity
                }).ToList()
            };

            await _orderService.AddAsync(order);
            await _cardItemService.Query()
                              .Where(c => c.UserId == userId)
                              .ExecuteDeleteAsync();

            var response = _mapper.Map<Order ,GetOrderResponseDto>(order);
            return Response<GetOrderResponseDto>.Success(response, HttpStatusCode.OK, "Sipariş oluşturuldu.");
        }

        public async Task<Response<IEnumerable<GetOrderResponseDto>>> GetAllByUserId(int userId)
        {
            var orders = await _orderService.Query()
              .Where(c => c.UserId == userId)
              .ToListAsync();
            var response = _mapper.Map<IEnumerable<Order>, IEnumerable<GetOrderResponseDto>>(orders);
            return Response<IEnumerable<GetOrderResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<GetOrderResponseDto>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
            {
                return Response<GetOrderResponseDto>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<Order, GetOrderResponseDto>(order);
                return Response<GetOrderResponseDto>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }
    }
}
