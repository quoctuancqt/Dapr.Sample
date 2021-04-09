using System.Collections.Generic;

namespace OrderService.Application.Dto
{
    public class CustomerBasketDto
    {
        public string BuyerId { get; set; }

        public IEnumerable<BasketItemDto> Items { get; set; }
    }
}
