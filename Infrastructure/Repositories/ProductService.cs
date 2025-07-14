using AutoMapper;
using backend.Application.DTOs.Filter;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using backend.Domain.Entities;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Web.Mvc;
using YirmibesYazilim.Framework.Models.Responses;

namespace backend.Infrastructure.Repositories
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;
        private readonly IBlobService _blob;

        public ProductService(IMapper mapper, IService<Product> service, IBlobService blob)
        {
            _mapper = mapper;
            _service = service;
            _blob = blob;
        }

        public async Task<Response<NoContent>> AddProductAsync(CreateProductRequestDto dto)
        {
            if (dto.Image == null || dto.Image.Length == 0) { 
                //return Response<NoContent>.Fail("Resim zorunlu.", HttpStatusCode.BadRequest);
                var productt = _mapper.Map<Product>(dto);
                productt.ImageUrl = "BU KISIM TEST İÇİN SİLİNECEK";

                await _service.AddAsync(productt);
                return Response<NoContent>.Success(HttpStatusCode.Created, "Ürün eklendi");
            }
            string url = await _blob.UploadAsync(dto.Image);

            var product = _mapper.Map<Product>(dto);
            product.ImageUrl = url;

            await _service.AddAsync(product);
            return Response<NoContent>.Success(HttpStatusCode.Created, "Ürün eklendi");
        }

        public async Task<Response<NoContent>> DeleteProductAsync(int productId)
        {
            var product = await _service.GetByIdAsync(productId);
            if (product == null)
                return Response<NoContent>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            if (!string.IsNullOrEmpty(product.ImageUrl))
                await _blob.DeleteAsync(Path.GetFileName(product.ImageUrl));

            await _service.DeleteAsync(productId);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Silindi");
        }

        public async Task<Response<IEnumerable<GetProductResponseDto>>> GetProductAllAsync(ProductFilter filter)
        {
            var query = _service.Query().AsQueryable();

            if (filter.CategoryId != null)
                query = query.Where(p => p.CategoryId == filter.CategoryId);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));

            if (filter.MinPrice != null)
                query = query.Where(p => p.Price >= filter.MinPrice);

            if (filter.MaxPrice != null)
                query = query.Where(p => p.Price <= filter.MaxPrice);

            if (filter.OrderByPriceAscending == true)
                query = query.OrderBy(p => p.Price);
            else if (filter.OrderByPriceAscending == false)
                query = query.OrderByDescending(p => p.Price);            

            var products = await query.ToListAsync();
            var response = _mapper.Map<IEnumerable<Product>, IEnumerable<GetProductResponseDto>>(products);
            return Response<IEnumerable<GetProductResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<GetProductResponseDto>> GetProductAsync(int productId)
        {
            var product = await _service.GetByIdAsync(productId);
            if (product == null)
            {
                return Response<GetProductResponseDto>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<Product, GetProductResponseDto>(product);
                return Response<GetProductResponseDto>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateProductAsync(UpdateProductRequestDto dto)
        {
            var product = await _service.GetByIdAsync(dto.Id);
            if (product == null)
                return Response<NoContent>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            _mapper.Map(dto, product);

            if (dto.Image is not null && dto.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                    await _blob.DeleteAsync(Path.GetFileName(product.ImageUrl));

                product.ImageUrl = await _blob.UploadAsync(dto.Image);
            }

            await _service.UpdateAsync(product);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Güncellendi");
        }

        public async Task<bool> IsProductNameExist(string name)
        {
            if (await _service.GetFirstOrDefaultAsync(p => p.Name == name) != null)
                return true;
            return false;
        }

        public async Task<Response<IEnumerable<GetProductResponseDto>>> GetProductByCategoryAsync(int categoryId)
        {
            var products = await _service.Query().Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name).ToListAsync();
            if (products == null)
            {
                return Response<IEnumerable<GetProductResponseDto>>.Fail("Ürün Yok.", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<IEnumerable<Product>, IEnumerable<GetProductResponseDto>>(products);
                return Response<IEnumerable<GetProductResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
            }
        }

        public async Task<Response<IEnumerable<GetProductResponseDto>>> GetProductByNameAsync(string name)
        {
            var products = await _service.Query().Where(p => p.Name.ToLower().Contains(name.ToLower())).OrderBy(p => p.Name).ToListAsync();
            var response = _mapper.Map<IEnumerable<Product>, IEnumerable<GetProductResponseDto>>(products);
            return Response<IEnumerable<GetProductResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }

        public async Task<Response<IEnumerable<GetProductResponseDto>>> GetProductByPriceRange(int minPrice, int maxPrice)
        {
            var products = await _service.Query().Where(p => p.Price>=minPrice && p.Price <= maxPrice).OrderBy(p => p.Price).ToListAsync();
            var response = _mapper.Map<IEnumerable<Product>, IEnumerable<GetProductResponseDto>>(products);
            return Response<IEnumerable<GetProductResponseDto>>.Success(response, HttpStatusCode.OK, "Başarılı!");
        }
    }
}   
