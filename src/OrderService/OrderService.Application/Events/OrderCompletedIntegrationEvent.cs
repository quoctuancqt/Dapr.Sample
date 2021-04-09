using OrderService.Application.Dto;
using SharedKernel.EventBus.Events;

namespace OrderService.Application.Events
{
    public class OrderCompletedIntegrationEvent : IntegrationEvent
    {
        public string OrderId { get; set; }
        public decimal Total { get; set; }
        public string BuyerId { get; set; }
        public AddressDto Address { get; set; }

        public OrderCompletedIntegrationEvent(string orderId, string buyerId, decimal total, AddressDto address)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            Total = total;
            Address = address;
        }
    }
}
