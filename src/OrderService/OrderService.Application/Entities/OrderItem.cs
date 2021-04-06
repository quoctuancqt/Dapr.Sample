using SharedKernel.Intefaces;

namespace OrderService.Application.Entities
{
    public class OrderItem : IBaseEntity
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quality { get; set; }
        public decimal Price { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
