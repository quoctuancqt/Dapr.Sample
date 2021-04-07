using System;

namespace OrderService.Application.Dto
{
    public class ProductDto
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
