using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Dto;
using ProductService.Application.Interfaces;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductQuerySearch querySearch)
        {
            return Ok(await _service.SearchAsync(querySearch));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
    }
}
