using ProductService.Application.Dto;
using SharedKernel;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IProductService
    {
        Task<Pageable<ProductDto>> SearchAsync(ProductQuerySearch querySearch);
        Task<ProductDto> GetByIdAsync(string id);
    }
}
