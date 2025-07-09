using backend.Application.DTOs.Category;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryService.GetCategoryAllAsync());
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _categoryService.GetCategoryAsync(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCategoryRequestDto dto)
        {
            return Ok(await _categoryService.AddCategoryAsync(dto));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateCategoryRequestDto dto)
        {
            return Ok(await _categoryService.UpdateCategoryAsync(dto));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _categoryService.DeleteCategoryAsync(id));
        }
    }
}
