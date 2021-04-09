using NotificationService.Models;
using SharedKernel.EventBus.Events;

namespace NotificationService.Application.Events
{
    public class OrderCompletedIntegrationEvent : IntegrationEvent
    {
        public string OrderId { get; set; }
        public decimal Total { get; set; }
        public string BuyerId { get; set; }
        public Address Address { get; set; }

        public OrderCompletedIntegrationEvent(string orderId, string buyerId, decimal total, Address address)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            Total = total;
            Address = address;
        }
    }
}
