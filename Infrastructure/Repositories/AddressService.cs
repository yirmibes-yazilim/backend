using AutoMapper;
using backend.Application.DTOs.Addresses;
using backend.Application.DTOs.CardItem;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly IService<Address> _service;
        public AddressService(IMapper mapper, IService<Address> service = null)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Response<NoContent>> AddAddressesAsync(CreateAddressesRequestDto category)
        {
            var newAddress = _mapper.Map<CreateAddressesRequestDto, Address>(category);
            await _service.AddAsync(newAddress);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Adres Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteAddressesAsync(int addressId)
        {
            var address = await _service.GetByIdAsync(addressId);
            if (address == null)
            {
                return Response<NoContent>.Fail("Silinecek adres bulunamadı.", HttpStatusCode.BadRequest);
            }
            await _service.DeleteAsync(addressId);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
        }

        public async Task<Response<IEnumerable<GetAddressesResponseDto>>> GetAddressesAllByUserIdAsync(int userId)
        {
            var addresses = await _service.Query().Where(c => c.UserId == userId).ToListAsync();
            if (addresses == null || !addresses.Any())
            {
                return Response<IEnumerable<GetAddressesResponseDto>>.Fail("Adres bulunamadı.", HttpStatusCode.NotFound);
            }
            var response = _mapper.Map<IEnumerable<Address>, IEnumerable<GetAddressesResponseDto>>(addresses);
            return Response<IEnumerable<GetAddressesResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<GetAddressesResponseDto>> GetAddressesAsync(int addressId)
        {
            var address = await _service.GetByIdAsync(addressId);
            if (address == null)
            {
                return Response<GetAddressesResponseDto>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<Address, GetAddressesResponseDto>(address);
                return Response<GetAddressesResponseDto>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateAddressesAsync(UpdateAddressesRequestDto newAddress)
        {
            var address = await _service.GetByIdAsync(newAddress.Id);
            if (address == null)
            {
                return Response<NoContent>.Fail("Güncellenecek ürün bulunamadı.", HttpStatusCode.BadRequest);
            }
            _mapper.Map(newAddress, address);
            await _service.UpdateAsync(address);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme başarılı!");
        }
    }
}
