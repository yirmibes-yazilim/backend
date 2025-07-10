using backend.Application.DTOs.CardItem;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardItemController : ControllerBase
    {
        private readonly ICardItemService _cardItemService;
        public CardItemController(ICardItemService cardItemService)
        {
            _cardItemService = cardItemService;
        }

        [HttpGet("getUsersCardItems/{userId}")]
        public async Task<IActionResult> GetUsersCardItems(int userId)
        {
            return Ok(await _cardItemService.GetCardItemsAllByUserIdAsync(userId));
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _cardItemService.GetCardItemByIdAsync(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCardItemRequestDto dto)
        {
            return Ok(await _cardItemService.AddCardItemAsync(dto));
        }

        [HttpPut("updateQuantity")]
        public async Task<IActionResult> Update(int cardItemId, int newQuantity)
        {
            return Ok(await _cardItemService.UpdateCardItemsQuantityAsync(cardItemId, newQuantity));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _cardItemService.DeleteCardItemAsync(id));
        }

        [HttpDelete("clearUsersCardItems/{userId}")]
        public async Task<IActionResult> ClearUsersCardItems(int userId)
        {
            return Ok(await _cardItemService.ClearCardItemsAllByUserIdAsync(userId));
        }
    }
}
