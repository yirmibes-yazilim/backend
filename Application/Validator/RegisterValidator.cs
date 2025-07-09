using backend.Application.DTOs.Auth;
using backend.Application.Services;
using backend.Infrastructure.Repositories;
using FluentValidation;
using System.Threading.Tasks;


namespace backend.Application.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDto>
    {
        private readonly IAuthService _authService;
        public RegisterValidator(IAuthService authService)
        {
            _authService = authService;

            RuleFor(x => x.FirstName).NotNull().WithMessage("İsim girilmesi zorunludur.")
                .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalı.")
                .MaximumLength(15).WithMessage("İsim en fazla 15 karakter olmalı");

            RuleFor(x => x.LastName).NotNull().WithMessage("Soyad girilmesi zorunludur.")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalı.")
                .MaximumLength(15).WithMessage("Soyad en fazla 15 karakter olmalı");

            RuleFor(x => x.Email).NotNull().WithMessage("Email girilmesi zorunludur.").EmailAddress();
                //.MustAsync(UniqueEmailAsync).WithMessage("Zaten hesabınız var."); 

            RuleFor(x => x.Password).NotNull().WithMessage("Şifre girilmesi zorunludur.")
                .MinimumLength(6).MaximumLength(100);
            
        }
        private async Task<bool> UniqueEmailAsync(string email, CancellationToken cancellationToken)
        {
            return !await _authService.IsEmailExist(email);
        }
    }
}
