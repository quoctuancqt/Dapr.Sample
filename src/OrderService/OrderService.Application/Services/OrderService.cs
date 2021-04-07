using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Dto;
using OrderService.Application.Entities;
using SharedKernel;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using SharedKernel.Intefaces;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrderService.Application.Services
{
    public class OrderService : IOrderService
    {
        private const string STORE_NAME = "statestore";
        private const string PUBSUB_NAME = "order-sub";
        private const string TOPIC_NAME = "order";


        private readonly IApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        private readonly DaprClient _client;

        public OrderService(IApplicationContext applicationContext, IMapper mapper, DaprClient client)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
            _client = client;
        }

        public async Task<OrderDto> CheckoutAsync(string buyerId, AddressDto dto)
        {
            var order = await _client.GetStateAsync<Order>(STORE_NAME, buyerId);

            if (order == null) throw new NotFoundException();

            var address = _mapper.Map<Address>(dto);
            order.SetAddress(address);
            order.CheckOut();

            _applicationContext.GetDbSet<Order>().Add(order);

            await _applicationContext.CommitAsync();

            await _client.DeleteStateAsync(STORE_NAME, buyerId);

            await _client.PublishEventAsync(PUBSUB_NAME, TOPIC_NAME, new { order.Id, order.Total, order.BuyerId, order.Address });

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateAsync(string buyerId, CreateOrEditOrderItemDto dto)
        {
            var order = new Order();
            order.SetBuyer(buyerId);

            //using var client = DaprClient.CreateInvokeHttpClient("productapp");
            //var product = await client.GetFromJsonAsync<ProductDto>($"/{dto.ProductId}");
            var product = await _client.InvokeMethodAsync<ProductDto>(HttpMethod.Get, "productapp", dto.ProductId);

            var orderItem = _mapper.Map<OrderItem>(dto);
            orderItem.OrderId = order.Id;
            orderItem.Price = product.Price;
            order.AddItem(orderItem);

            await _client.SaveStateAsync(STORE_NAME, buyerId, order);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> EditAsync(string buyerId, CreateOrEditOrderItemDto dto)
        {
            var order = await _client.GetStateAsync<Order>(STORE_NAME, buyerId);

            if (order == null) throw new NotFoundException();

            using var client = DaprClient.CreateInvokeHttpClient("productapp");
            var product = await client.GetFromJsonAsync<ProductDto>($"/{dto.ProductId}");

            var orderItem = _mapper.Map<OrderItem>(dto);
            orderItem.OrderId = order.Id;
            orderItem.Price = product.Price;
            order.AddItem(orderItem);

            await _client.SaveStateAsync(STORE_NAME, buyerId, order);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> GetByIdAsync(string id)
        {
            var order = await _applicationContext.GetDbSet<Order>().Include(x => x.Items).SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (order == null) throw new NotFoundException(id);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task RemoveAsync(string id)
        {
            var order = await _applicationContext.GetDbSet<Order>().FindAsync(id);

            if (order == null) throw new NotFoundException(id);

            _applicationContext.GetDbSet<Order>().Remove(order);

            await _applicationContext.CommitAsync();
        }

        public async Task<OrderDto> RemoveItemAsync(string buyerId, string itemId)
        {
            var order = await _client.GetStateAsync<Order>(STORE_NAME, buyerId);

            if (order == null) throw new NotFoundException();

            order.RemoveItem(itemId);

            await _client.SaveStateAsync(STORE_NAME, buyerId, order);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<Pageable<OrderDto>> SearchAsync(OrderQuerySearch querySearch)
        {
            var query = _applicationContext.GetQuery<Order>(querySearch).ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var totalItem = query.Count();

            var items = await query.ApplyPaging(querySearch.GetSkip(), querySearch.GetTake()).ToListAsync();

            return new Pageable<OrderDto>(totalItem, querySearch.GetTake(), querySearch.PageIndex, items);
        }
    }
}
