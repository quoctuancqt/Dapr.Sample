using AutoMapper;
using OrderService.Application.Entities;
using SharedKernel.Mapping;

namespace OrderService.Application.Dto
{
    public class OrderDto : IMapFrom<Order>
    {
        public string BuyerId { get; set; }
        public decimal Total { get; set; }
        public AddressDto Address { get; set; }
        public string Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>()
                .ForMember(x => x.Status, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Status = src.Status.ToString();
                });
        }
    }
}
