using EcommerceAPI.DTOs;
using FluentValidation;

namespace EcommerceAPI.Validators
{
    public class AddToCartValidator
        : AbstractValidator<AddToCartDto>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("CustomerId must be greater than 0.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("Quantity cannot exceed 100.");
        }
    }
}