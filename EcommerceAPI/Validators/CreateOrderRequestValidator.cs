using EcommerceAPI.DTOs;
using FluentValidation;

namespace EcommerceAPI.Validators
{
    public class CreateOrderRequestValidator
        : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("CustomerId must be greater than 0.");

            RuleFor(x => x.ShippingMethodId)
                .GreaterThan(0)
                .WithMessage("ShippingMethodId must be greater than 0.");

            RuleFor(x => x.ShippingName)
                .NotEmpty()
                .WithMessage("Shipping name is required.")
                .MaximumLength(200)
                .WithMessage("Shipping name cannot exceed 200 characters.");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty()
                .WithMessage("Shipping address is required.")
                .MaximumLength(500)
                .WithMessage("Shipping address cannot exceed 500 characters.");

            RuleFor(x => x.ShippingCity)
                .NotEmpty()
                .WithMessage("Shipping city is required.")
                .MaximumLength(100)
                .WithMessage("Shipping city cannot exceed 100 characters.");

            RuleFor(x => x.ShippingState)
                .NotEmpty()
                .WithMessage("Shipping state is required.")
                .MaximumLength(100)
                .WithMessage("Shipping state cannot exceed 100 characters.");

            RuleFor(x => x.ShippingPostalCode)
                .NotEmpty()
                .WithMessage("Shipping postal code is required.")
                .MaximumLength(20)
                .WithMessage("Shipping postal code cannot exceed 20 characters.");

            RuleFor(x => x.ShippingPhone)
                .NotEmpty()
                .WithMessage("Shipping phone is required.")
                .MaximumLength(30)
                .WithMessage("Shipping phone cannot exceed 30 characters.");

            RuleFor(x => x)
                .Must(HaveValidShippingDetails)
                .WithMessage(
                    "Shipping address, city and state must all be provided together.");
        }

        private static bool HaveValidShippingDetails(
            CreateOrderRequestDto request)
        {
            bool addressProvided =
                !string.IsNullOrWhiteSpace(request.ShippingAddress);

            bool cityProvided =
                !string.IsNullOrWhiteSpace(request.ShippingCity);

            bool stateProvided =
                !string.IsNullOrWhiteSpace(request.ShippingState);

            return addressProvided == cityProvided
                && cityProvided == stateProvided;
        }
    }
}