using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
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

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetProductAllAsync());
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetProductAsync(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductRequestDto dto)
        {
            return Ok(await _productService.AddProductAsync(dto));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductRequestDto dto)
        {
            return Ok(await _productService.UpdateProductAsync(dto));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _productService.DeleteProductAsync(id));
        }
    }
}
