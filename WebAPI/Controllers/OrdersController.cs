using backend.Application.DTOs.Order;
using backend.Application.Services;
using backend.Domain.Entities;
using backend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("confirmCard")]
        public async Task<ActionResult<GetOrderResponseDto>> ConfrimCard()
        {
            return Ok(await _orderService.ConfirmCardAsync());
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult<GetOrderResponseDto>> GetById(int id)
        {
            return Ok(await _orderService.GetOrderById(id));
        }

        [HttpGet("getUsersOrders")]
        public async Task<IActionResult> GetUsersCardItems()
        {
            return Ok(await _orderService.GetAllByUserId());
        }

    }
}
