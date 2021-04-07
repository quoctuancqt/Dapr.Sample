using AutoMapper;
using OrderService.Application.Entities;
using SharedKernel.Mapping;

namespace OrderService.Application.Dto
{
    public class OrderItemDto : IMapFrom<OrderItem>
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quality { get; set; }
        public decimal Price { get; set; }
        public string OrderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
