using OrderService.Application.Dto;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Services
{
    public interface IOrderService
    {
        Task<Pageable<OrderDto>> SearchAsync(OrderQuerySearch querySearch);
        Task<OrderDto> GetByIdAsync(string id);
        Task<CustomerBasketDto> GetBasketAsync(string customerId, CancellationToken cancellationToken = default);
        Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket, CancellationToken cancellationToken = default);
        Task DeleteBasketAsync(string id, CancellationToken cancellationToken = default);
        Task<OrderDto> CheckoutAsync(string buyerId, AddressDto dto);
    }
}
