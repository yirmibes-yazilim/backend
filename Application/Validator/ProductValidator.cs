using backend.Application.DTOs.Auth;
using backend.Application.DTOs.Product;
using backend.Application.Services;
using FluentValidation;

namespace backend.Application.Validator
{
    public class ProductValidator : AbstractValidator<CreateProductRequestDto>
    {
        private readonly IProductService _productService;
        public ProductValidator(IProductService productService)
        {
            _productService = productService;


            RuleFor(x => x.Name).NotNull().WithMessage("İsim girilmesi zorunludur.")
                .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalı.")
                .MaximumLength(100).WithMessage("İsim en fazla 100 karakter olmalı");
                //.MustAsync(UniqueNameAsync).WithMessage("Zaten ürün var.");

            RuleFor(x => x.Description).NotNull().WithMessage("Açıklama girilmesi zorunludur.")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalı.")
                .MaximumLength(500).WithMessage("Soyad en fazla 500 karakter olmalı");

            RuleFor(x => x.Stock)
                .NotNull().WithMessage("Stok girilmesi zorunludur.")
                .Must(s => s >= 0).WithMessage("Stok negatif olamaz.");

            RuleFor(x => x.Price).NotNull().WithMessage("Fiyat girilmesi zorunludur.")
                .Must(s => s > 0).WithMessage("Fiyat 0'dan büyük olmalı");
            
        }
        private async Task<bool> UniqueNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _productService.IsProductNameExist(name);
        }
    }
}
