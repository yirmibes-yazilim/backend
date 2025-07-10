using backend.Application.DTOs.Order;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IOrderService
    {
        Task<Response<GetOrderResponseDto>> ConfirmCardAsync(int userId);
        Task<Response<GetOrderResponseDto>> GetOrderById(int orderId);
        Task<Response<IEnumerable<GetOrderResponseDto>>> GetAllByUserId(int userId);

    }
}
