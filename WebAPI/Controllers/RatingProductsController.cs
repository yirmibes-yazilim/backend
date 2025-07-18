using backend.Application.DTOs.RatingProduct;
using backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingProductsController : ControllerBase
    {
        private readonly IRatingProductService _ratingProductService;

        public RatingProductsController(IRatingProductService ratingProductService)
        {
            _ratingProductService = ratingProductService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddRatingAsync(CreateRatingProductRequestDto createRatingProductRequestDto)
        {
            return Ok(await _ratingProductService.AddRatingAsync(createRatingProductRequestDto));
        }
        [HttpGet("getUserRating/{productId}/{userId}")]
        public async Task<IActionResult> GetUserRatingAsync(int productId, int userId)
        {
            return Ok(await _ratingProductService.GetUserRatingAsync(productId, userId));
        }
        [HttpGet("getRatingsByProduct/{productId}")]
        public async Task<IActionResult> GetRatingsByProductAsync(int productId)
        {
            return Ok(await _ratingProductService.GetRatingsByProductAsync(productId));
        }
        [HttpGet("getRatingsByUser/{userId}")]
        public async Task<IActionResult> GetRatingsByUserAsync(int userId)
        {
            return Ok(await _ratingProductService.GetRatingsByUserAsync(userId));
        }
        [HttpDelete("deleteRating/{productId}/{userId}")]
        public async Task<IActionResult> DeleteRatingAsync(int productId, int userId)
        {
            return Ok(await _ratingProductService.DeleteRatingAsync(productId, userId));
        }
    }
}
