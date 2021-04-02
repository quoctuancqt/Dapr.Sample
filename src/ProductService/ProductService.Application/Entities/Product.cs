using SharedKernel;

namespace BasketService.Application.Entities
{
    public class Product : BaseEntity
    {
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
