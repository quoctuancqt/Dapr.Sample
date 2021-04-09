using SharedKernel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Application.Entities
{
    public class Order : BaseEntity
    {
        public string BuyerId { get; set; }
        public decimal Total { get; set; }
        public Address Address { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
