using EcommerceAPI.DTOs;
using FluentValidation;

namespace EcommerceAPI.Validators
{
    public class ProductRequestValidator
        : AbstractValidator<ProductRequestDto>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0)
                .WithMessage("BrandId must be greater than 0.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating must be between 0 and 5.");
        }
    }
}