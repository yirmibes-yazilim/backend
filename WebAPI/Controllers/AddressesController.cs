using backend.Application.DTOs.Addresses;
using backend.Application.DTOs.Category;
using backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressesService;

        public AddressesController(IAddressService addressesService)
        {
            _addressesService = addressesService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _addressesService.GetAddressesAllAsync());
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _addressesService.GetAddressesAsync(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateAddressesRequestDto dto)
        {
            return Ok(await _addressesService.AddAddressesAsync(dto));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateAddressesRequestDto dto)
        {
            return Ok(await _addressesService.UpdateAddressesAsync(dto));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _addressesService.DeleteAddressesAsync(id));
        }
    }
}
