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

        [HttpPost("buyer/{buyerId}/order:new")]
        public async Task<IActionResult> CreateOrder(string buyerId, [FromBody] CreateOrEditOrderItemDto dto)
            => Ok(await _service.CreateAsync(buyerId, dto));

        [HttpPut("buyer/{buyerId}/order:update")]
        public async Task<IActionResult> UpdateOrder(string buyerId, [FromBody] CreateOrEditOrderItemDto dto)
            => Ok(await _service.EditAsync(buyerId, dto));

        [HttpPut("buyer/{buyerId}/order:checkout")]
        public async Task<IActionResult> CheckoutOrder(string buyerId, [FromBody] AddressDto dto)
            => Ok(await _service.CheckoutAsync(buyerId, dto));
    }
}
