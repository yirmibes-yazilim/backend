using backend.Application.DTOs.Addresses;
using backend.Application.DTOs.Category;
using Microsoft.AspNetCore.Http.HttpResults;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Application.Services
{
    public interface IAddressService
    {
        Task<Response<GetAddressesResponseDto>> GetAddressesAsync(int addressId);
        Task<Response<NoContent>> AddAddressesAsync(CreateAddressesRequestDto address);
        Task<Response<NoContent>> UpdateAddressesAsync(UpdateAddressesRequestDto newAddress);
        Task<Response<NoContent>> DeleteAddressesAsync(int addressId);
        Task<Response<IEnumerable<GetAddressesResponseDto>>> GetAddressesAllByUserIdAsync();
        Task<Response<NoContent>> SetAddressDefaultAsync(int addressId);
    }
}
