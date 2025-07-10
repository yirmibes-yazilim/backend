using backend.Application.DTOs.Order;
using backend.Application.Services;
using backend.Domain.Entities;
using backend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("confirmCard/{userId}")]
        public async Task<ActionResult<GetOrderResponseDto>> ConfrimCard(int userId)
        {
            return Ok(await _orderService.ConfirmCardAsync(userId));
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult<GetOrderResponseDto>> GetById(int id)
        {
            return Ok(await _orderService.GetOrderById(id));
        }

        [HttpGet("getUsersOrders/{userId}")]
        public async Task<IActionResult> GetUsersCardItems(int userId)
        {
            return Ok(await _orderService.GetAllByUserId(userId));
        }

    }
}
