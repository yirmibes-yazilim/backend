using AutoMapper;
using backend.Application.DTOs.Category;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IService<Category> _service;
        public CategoryService(IMapper mapper, IService<Category> service = null)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Response<NoContent>> AddCategoryAsync(CreateCategoryRequestDto category)
        {
            var newCategory = _mapper.Map<CreateCategoryRequestDto, Category>(category);
            await _service.AddAsync(newCategory);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Kategori Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteCategoryAsync(int productId)
        {
            await _service.DeleteAsync(productId);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
        }

        public async Task<Response<IEnumerable<GetCategoryResponseDto>>> GetCategoryAllAsync()
        {
            var categories = await _service.GetAllAsync();
            var response = _mapper.Map<IEnumerable<Category>, IEnumerable<GetCategoryResponseDto>>(categories);
            return Response<IEnumerable<GetCategoryResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<GetCategoryResponseDto>> GetCategoryAsync(int categoryId)
        {
            var category = await _service.GetByIdAsync(categoryId);
            if (category == null)
            {
                return Response<GetCategoryResponseDto>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<Category, GetCategoryResponseDto>(category);
                return Response<GetCategoryResponseDto>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateCategoryAsync(UpdateCategoryRequestDto newCategory)
        {
            var category = await _service.GetByIdAsync(newCategory.Id);
            if (category == null)
            {
                return Response<NoContent>.Fail("Güncellenecek ürün bulunamadı.", HttpStatusCode.BadRequest);
            }
            _mapper.Map(newCategory, category);
            await _service.UpdateAsync(category);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme başarılı!");
        }
    }
}
