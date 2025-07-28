using backend.Application.DTOs.FavoriteProduct;
using backend.Application.DTOs.Filter;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteProductsController : ControllerBase
    {
        private readonly IFavoriteProductService _favoriteProductService;

        public FavoriteProductsController(IFavoriteProductService favoriteProductService)
        {
            _favoriteProductService = favoriteProductService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            return Ok(await _favoriteProductService.GetUserFavoriteProductAll());
        }

        [HttpPost("addFavorite")]
        public async Task<IActionResult> Create(CreateFavoriteProductRequestDto dto)
        {
            return Ok(await _favoriteProductService.AddFavoriteProductAsync(dto));
        }
        [HttpDelete("removeFavorite")]
        public async Task<IActionResult> RemoveFavorite(RemoveFavoriteProductRequestDto dto)
        {
            return Ok(await _favoriteProductService.RemoveFavoriteProductAsync(dto));
        }
    }
}
