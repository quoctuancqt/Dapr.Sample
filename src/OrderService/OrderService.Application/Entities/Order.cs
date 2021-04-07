using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Application.Entities
{
    public class Order : BaseEntity
    {
        public string BuyerId { get; private set; }
        private decimal _total => Items.Sum(x => x.Price * x.Quality);
        public decimal Total => _total;
        public Address Address { get; private set; }
        public OrderStatus Status { get; private set; }
        public virtual ICollection<OrderItem> Items { get; private set; }

        public Order()
        {
            Id = Guid.NewGuid().ToString();
            Items = new List<OrderItem>();
            Status = OrderStatus.Pending;
        }

        public void SetBuyer(string buyerId)
        {
            BuyerId = buyerId;
        }

        public void AddItem(OrderItem item)
        {
            if (!Items.Any(x => x.ProductId.Equals(item.ProductId)))
            {
                Items.Add(item);
            }
            else
            {
                var dbItem = Items.SingleOrDefault(x => x.ProductId.Equals(item.ProductId));
                dbItem.Quality += item.Quality;
            }
        }

        public void RemoveItem(string itemId)
        {
            var item = Items.SingleOrDefault(x => x.ProductId.Equals(itemId, StringComparison.OrdinalIgnoreCase));

            if (item == null) throw new Exception($"Not found item with id {itemId}");

            Items.Remove(item);
        }

        public void CheckOut()
        {
            Status = OrderStatus.Completed;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }
    }
}
