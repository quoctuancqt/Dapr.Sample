using SharedKernel;
using System.Collections.Generic;

namespace OrderService.Application.Entities
{
    public class Order : BaseEntity
    {
        public decimal Total { get; set; }
        public Address Address { get; set; }
        public OrderStatus Status { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
