using System.Collections.Generic;

namespace OrderService.Application.Dto
{
    public class CustomerBasketDto
    {
        public string BuyerId { get; set; }

        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        public CustomerBasketDto()
        {

        }

        public CustomerBasketDto(string customerId)
        {
            BuyerId = customerId;
        }
    }
}
