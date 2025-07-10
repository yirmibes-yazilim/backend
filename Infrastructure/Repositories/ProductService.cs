using AutoMapper;
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
        public ProductService(IMapper mapper, IService<Product> service = null)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Response<NoContent>> AddProductAsync(CreateProductRequestDto product)
        {
            var newProduct = _mapper.Map<CreateProductRequestDto, Product>(product);
            await _service.AddAsync(newProduct);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Ürün Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteProductAsync(int productId)
        {
            await _service.DeleteAsync(productId);
            return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
        }

        public async Task<Response<IEnumerable<GetProductResponseDto>>> GetProductAllAsync()
        {
            var products = await _service.GetAllAsync();
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

        public async Task<Response<NoContent>> UpdateProductAsync(UpdateProductRequestDto newProduct)
        {
            var product = await _service.GetByIdAsync(newProduct.Id);
            if (product == null)
            {
                return Response<NoContent>.Fail("Güncellenecek ürün bulunamadı.", HttpStatusCode.BadRequest);
            }
            _mapper.Map(newProduct, product); 
            await _service.UpdateAsync(product); 
            return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme başarılı!");
        }

        public async Task<bool> IsProductNameExist(string name)
        {
            if (await _service.GetFirstOrDefaultAsync(p => p.Name == name) != null)
                return true;
            return false;
        }
    }
}   
