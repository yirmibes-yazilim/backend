using AutoMapper;
using backend.Application.DTOs.Addresses;
using backend.Application.DTOs.CardItem;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using YirmibesYazilim.Framework.Models.Responses;
using backend.Infrastructure.Extensions;

namespace backend.Infrastructure.Repositories
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly IService<Address> _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddressService(IMapper mapper, IService<Address> service = null, IHttpContextAccessor httpContextAccessor = null)
        {
            _mapper = mapper;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<NoContent>> AddAddressesAsync(CreateAddressesRequestDto addressesRequestDto)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            //if(userId != addressesRequestDto.UserId)
            //{
            //    return Response<NoContent>.Fail("Bu adres sizin değil.", HttpStatusCode.BadRequest);
            //}
            var newAddress = _mapper.Map<CreateAddressesRequestDto, Address>(addressesRequestDto);
            newAddress.UserId = userId;
            await _service.AddAsync(newAddress);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Adres Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteAddressesAsync(int addressId)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            if(await _service.Query().AnyAsync(c => c.Id == addressId && c.UserId == userId))
            {
                return Response<NoContent>.Fail("Bu adres sizin değil.", HttpStatusCode.BadRequest);
            }
            var address = await _service.GetByIdAsync(addressId);
            if (address == null)
            {
                return Response<NoContent>.Fail("Silinecek adres bulunamadı.", HttpStatusCode.BadRequest);
            }
            await _service.DeleteAsync(addressId);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
        }

        public async Task<Response<IEnumerable<GetAddressesResponseDto>>> GetAddressesAllByUserIdAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            //if (claimUserId != userId)
            //{
            //    return Response<IEnumerable<GetAddressesResponseDto>>.Fail("Bu adresler sizin değil.", HttpStatusCode.BadRequest);
            //}
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
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (!await _service.Query().AnyAsync(c => c.Id == addressId && c.UserId == userId))
            {
                return Response<GetAddressesResponseDto>.Fail("Bu adres sizin değil.", HttpStatusCode.BadRequest);
            }
            var address = await _service.GetByIdAsync(addressId);
            if (address == null)
            {
                return Response<GetAddressesResponseDto>.Fail("Adres Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<Address, GetAddressesResponseDto>(address);
                return Response<GetAddressesResponseDto>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateAddressesAsync(UpdateAddressesRequestDto newAddress)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            //if (userId != newAddress.UserId)
            //{
            //    return Response<NoContent>.Fail("Bu adres sizin değil.", HttpStatusCode.BadRequest);
            //}
            var address = await _service.GetByIdAsync(newAddress.Id);
            if (address == null)
            {
                return Response<NoContent>.Fail("Güncellenecek ürün bulunamadı.", HttpStatusCode.BadRequest);
            }
            _mapper.Map(newAddress, address);
            await _service.UpdateAsync(address);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme başarılı!");
        }
        public async Task<Response<NoContent>> SetAddressDefaultAsync(int addressId)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            //if (claimUserId != userId)
            //{
            //    return Response<NoContent>.Fail("Bu adres sizin değil.", HttpStatusCode.BadRequest);
            //}
            var address = await _service.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId)
            {
                return Response<NoContent>.Fail("Adres bulunamadı veya kullanıcıya ait değil.", HttpStatusCode.BadRequest);
            }
            var addresses = await _service.Query().Where(a => a.UserId == userId).ToListAsync();
            foreach (var addr in addresses)
            {
                addr.IsDefault = false; 
            }
            address.IsDefault = true;
            await _service.UpdateAsync(address);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Adres seçimi başarılı!");
        }
    }
}
