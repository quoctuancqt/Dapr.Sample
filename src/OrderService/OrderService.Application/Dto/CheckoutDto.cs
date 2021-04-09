using FluentValidation;

namespace OrderService.Application.Dto
{
    public class CheckoutDto
    {
        public string BuyerId { get; set; }
        public AddressDto Address { get; set; }
    }

    public class CheckoutDtoValidator : AbstractValidator<CheckoutDto>
    {
        public CheckoutDtoValidator()
        {
            RuleFor(x => x.BuyerId).NotNull().NotEmpty();
            RuleFor(x => x.Address).NotNull().SetValidator(new AddressDtoValidator());
        }
    }
}
