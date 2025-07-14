using AutoMapper;
using backend.Application.DTOs.FavoriteProduct;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class FavoriteProductService : IFavoriteProductService
    {
        private readonly IMapper _mapper;
        private readonly IService<FavoriteProduct> _favoriteService;

        public FavoriteProductService(IMapper mapper, IService<FavoriteProduct> favoriteService)
        {
            _mapper = mapper;
            _favoriteService = favoriteService;
        }

        public async Task<Response<NoContent>> AddFavoriteProductAsync(CreateFavoriteProductRequestDto req)
        {
            var favoriteProduct = _mapper.Map<CreateFavoriteProductRequestDto, FavoriteProduct>(req);
            await _favoriteService.AddAsync(favoriteProduct);
            return Response<NoContent>.Success(System.Net.HttpStatusCode.Created, "Favori ürün eklendi");
        }

        public async Task<Response<IEnumerable<GetProductResponseDto>>> GetUserFavoriteProductAll(int userId)
        {
            var query = _favoriteService.Query().Where(u => u.UserId == userId).Select(fp => fp.Product);
            var favoriteProducts = await query.ToListAsync();
            if (favoriteProducts == null || !favoriteProducts.Any())
                return Response<IEnumerable<GetProductResponseDto>>.Fail("Favori ürün bulunamadı", System.Net.HttpStatusCode.NotFound);
            var response = _mapper.Map<IEnumerable<GetProductResponseDto>>(favoriteProducts);
            return Response<IEnumerable<GetProductResponseDto>>.Success(response, System.Net.HttpStatusCode.OK, "Favori ürünler başarıyla getirildi");
        }

        public async Task<Response<NoContent>> RemoveFavoriteProductAsync(RemoveFavoriteProductRequestDto req)
        {
            var favoriteProduct = _favoriteService.GetFirstOrDefaultAsync(x => x.UserId == req.UserId && x.ProductId == req.ProductId);
            if (favoriteProduct == null)
            {
                return Response<NoContent>.Fail("Favori ürün bulunamadı", System.Net.HttpStatusCode.NotFound);
            }
            await _favoriteService.DeleteAsync(favoriteProduct.Result.Id);
            return Response<NoContent>.Success(System.Net.HttpStatusCode.OK, "Favori ürün başarıyla kaldırıldı");
        }
    }
}
