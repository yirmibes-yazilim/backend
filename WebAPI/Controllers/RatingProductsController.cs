using backend.Application.DTOs.RatingProduct;
using backend.Application.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> AddRatingAsync(CreateRatingProductRequestDto createRatingProductRequestDto)
        {
            return Ok(await _ratingProductService.AddRatingAsync(createRatingProductRequestDto));
        }
        [HttpGet("getUserRating/{productId}")]
        [Authorize]
        public async Task<IActionResult> GetUserRatingAsync(int productId)
        {
            return Ok(await _ratingProductService.GetUserProductRatingsAsync(productId));
        }
        [HttpGet("getRatingsByProduct/{productId}")]
        public async Task<IActionResult> GetRatingsByProductAsync(int productId)
        {
            return Ok(await _ratingProductService.GetRatingsByProductAsync(productId));
        }
        [HttpGet("getRatingsByUser")]
        [Authorize]
        public async Task<IActionResult> GetRatingsByUserAsync()
        {
            return Ok(await _ratingProductService.GetRatingsByUserAsync());
        }
        [HttpDelete("deleteRating/{productId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRatingAsync(int productId)
        {
            return Ok(await _ratingProductService.DeleteRatingAsync(productId));
        }
    }
}
