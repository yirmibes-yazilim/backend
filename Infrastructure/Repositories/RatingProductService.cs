using AutoMapper;
using backend.Application.DTOs.RatingProduct;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class RatingProductService : IRatingProductService
    {
        private readonly IService<RatingProduct> _service;
        private readonly IMapper _mapper;

        public RatingProductService(IService<RatingProduct> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<Response<NoContent>> AddRatingAsync(CreateRatingProductRequestDto createRatingProductRequestDto)
        {
            var rating = _mapper.Map<RatingProduct>(createRatingProductRequestDto);
            await _service.AddAsync(rating);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kullanıcı değerlendirmesi başarılı");
        }

        public async Task<Response<NoContent>> DeleteRatingAsync(int productId, int userId)
        {
            var rating = _service.GetFirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
            if (rating == null)
            {
                return Response<NoContent>.Fail("Değerlendirme bulunamadı", HttpStatusCode.NotFound);
            }
            await _service.DeleteAsync(rating.Id);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kullanıcı değerlendirmesi silindi");
        }

        public async Task<Response<IEnumerable<GetRatingProductResponseDto>>> GetRatingsByProductAsync(int productId)
        {
            var ratings = await _service.Query().Where(r => r.ProductId == productId).ToListAsync();
            if (ratings == null)
            {
                return Response<IEnumerable<GetRatingProductResponseDto>>.Fail("Ürün için değerlendirme bulunamadı", HttpStatusCode.NotFound);
            }
            var ratingDtos = _mapper.Map<IEnumerable<RatingProduct>, IEnumerable<GetRatingProductResponseDto>>(ratings);
            return Response<IEnumerable<GetRatingProductResponseDto>>.Success(ratingDtos, HttpStatusCode.OK, "Ürün değerlendirmeleri başarıyla getirildi");
        }

        public async Task<Response<IEnumerable<GetRatingProductResponseDto>>> GetRatingsByUserAsync(int userId)
        {
            var ratings = await _service.Query().Where(r => r.UserId == userId).ToListAsync();
            if (ratings == null)
            {
                return Response<IEnumerable<GetRatingProductResponseDto>>.Fail("Ürün için değerlendirme bulunamadı", HttpStatusCode.NotFound);
            }
            var ratingDtos = _mapper.Map<IEnumerable<GetRatingProductResponseDto>>(ratings);
            return Response<IEnumerable<GetRatingProductResponseDto>>.Success(ratingDtos, HttpStatusCode.OK, "Ürün değerlendirmeleri başarıyla getirildi");
        }

        public async Task<Response<GetRatingProductResponseDto>> GetUserRatingAsync(int productId, int userId)
        {
            var rating = _service.GetFirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
            if (rating == null)
            {
                return Response<GetRatingProductResponseDto>.Fail("Değerlendirme bulunamadı", HttpStatusCode.NotFound);
            }
            var ratingDto = _mapper.Map<GetRatingProductResponseDto>(rating);
            return Response<GetRatingProductResponseDto>.Success(ratingDto, HttpStatusCode.OK, "Kullanıcı değerlendirmesi başarıyla getirildi");
        }
    }
}
