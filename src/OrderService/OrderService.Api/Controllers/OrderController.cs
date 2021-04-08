using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Dto;
using OrderService.Application.Services;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] OrderQuerySearch querySearch)
            => Ok(await _service.SearchAsync(querySearch));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPut(":checkout")]
        public async Task<IActionResult> CheckoutOrder(string buyerId, [FromBody] AddressDto dto)
            => Ok(await _service.CheckoutAsync(buyerId, dto));

        [HttpGet("basket/{id}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketByIdAsync(string id)
        {
            var basket = await _service.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasketDto(id));
        }

        [HttpPost("basket")]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync([FromBody] CustomerBasketDto dto)
            => Ok(await _service.UpdateBasketAsync(dto));

        [HttpDelete("basket/{id}")]
        public async Task DeleteBasketByIdAsync(string id)
            => await _service.DeleteBasketAsync(id);
    }
}
