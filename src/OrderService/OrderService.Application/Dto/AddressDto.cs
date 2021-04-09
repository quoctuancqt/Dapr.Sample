using AutoMapper;
using FluentValidation;
using OrderService.Application.Entities;
using SharedKernel.Mapping;

namespace OrderService.Application.Dto
{
    public class AddressDto : IMapFrom<Address>
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddressDto, Address>().ReverseMap();
        }
    }

    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Line1).NotNull().NotEmpty();
            RuleFor(x => x.City).NotNull().NotEmpty();
            RuleFor(x => x.Province).NotNull().NotEmpty();
            RuleFor(x => x.Country).NotNull().NotEmpty();
        }
    }
}
