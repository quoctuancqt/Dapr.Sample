using AutoMapper;
using FluentValidation;
using OrderService.Application.Entities;
using SharedKernel.Mapping;
using System.Text.Json.Serialization;

namespace OrderService.Application.Dto
{
    public class CreateOrEditOrderItemDto : IMapFrom<OrderItem>
    {
        public string ProductId { get; set; }
        public int Quality { get; set; }
        [JsonIgnore]
        public string OrderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrEditOrderItemDto, OrderItem>()
                    .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }

    public class CreateOrEditOrderItemDtoValidator : AbstractValidator<CreateOrEditOrderItemDto>
    {
        public CreateOrEditOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.Quality).GreaterThan(0);
        }
    }
}
