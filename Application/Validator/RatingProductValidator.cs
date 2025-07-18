using backend.Application.DTOs.Product;
using backend.Application.DTOs.RatingProduct;
using FluentValidation;

namespace backend.Application.Validator
{
    public class RatingProductValidator : AbstractValidator<CreateRatingProductRequestDto>
    {
        public RatingProductValidator()
        {
            RuleFor(x => x.Rating)
                .NotNull().WithMessage("Yıldız vermek zorunludur.")
                .InclusiveBetween(1, 5).WithMessage("Yıldız 1 ile 5 arasında olmalı.");

            RuleFor(x => x.Comment)
                .NotNull().WithMessage("Yorum girilmesi zorunludur.")
                .MinimumLength(0).WithMessage("Yorum en az 0 karakter olmalı.")
                .MaximumLength(500).WithMessage("Yorum en fazla 500 karakter olmalı.");
        }
    }
}
