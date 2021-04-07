using OrderService.Application.Dto;
using SharedKernel;
using System.Threading.Tasks;

namespace OrderService.Application.Services
{
    public interface IOrderService
    {
        Task<Pageable<OrderDto>> SearchAsync(OrderQuerySearch querySearch);
        Task<OrderDto> GetByIdAsync(string id);
        Task<OrderDto> CreateAsync(string buyerId, CreateOrEditOrderItemDto dto);
        Task<OrderDto> EditAsync(string buyerId, CreateOrEditOrderItemDto dto);
        Task RemoveAsync(string id);
        Task<OrderDto> RemoveItemAsync(string buyerId, string itemId);
        Task<OrderDto> CheckoutAsync(string buyerId, AddressDto dto);
    }
}
