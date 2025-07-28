using backend.Application.DTOs.Filter;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] ProductFilter filter)
        {
            return Ok(await _productService.GetProductAllAsync(filter));
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetProductAsync(id));
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateProductRequestDto dto)
        {
            return Ok(await _productService.AddProductAsync(dto));
        }

        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequestDto dto)
        {
            return Ok(await _productService.UpdateProductAsync(dto));
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _productService.DeleteProductAsync(id));
        }
    }
}
