using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Dto;
using OrderService.Application.Services;
using System.Net.Http.Json;
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

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutDto dto)
            => Ok(await _service.CheckoutAsync(dto.BuyerId, dto.Address));

        [HttpGet("basket/{id}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketByIdAsync(string id)
        {
            var basket = await _service.GetBasketAsync(id);

            if (basket == null) return NotFound();

            return Ok(basket);
        }

        [HttpPost("basket")]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync([FromBody] CustomerBasketDto dto)
            => Ok(await _service.UpdateBasketAsync(dto));

        [HttpDelete("basket/{id}")]
        public async Task DeleteBasketByIdAsync(string id)
            => await _service.DeleteBasketAsync(id);

        // This API will use to test Service-To-Service communication
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var client = DaprClient.CreateInvokeHttpClient("productservice");
            return Ok(await client.GetFromJsonAsync<ProductDto>($"product/{id}"));
        }
    }
}
