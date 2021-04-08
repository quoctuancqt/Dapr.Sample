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
using System.Threading;
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
        private readonly DaprClient _dapr;

        public OrderService(IApplicationContext applicationContext, IMapper mapper, DaprClient client)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
            _dapr = client;
        }

        public async Task<OrderDto> CheckoutAsync(string buyerId, AddressDto dto)
        {
            var order = await _dapr.GetStateAsync<Order>(STORE_NAME, buyerId);

            if (order == null) throw new NotFoundException();

            var address = _mapper.Map<Address>(dto);
            order.SetAddress(address);
            order.CheckOut();

            _applicationContext.GetDbSet<Order>().Add(order);

            await _applicationContext.CommitAsync();

            await _dapr.DeleteStateAsync(STORE_NAME, buyerId);

            await _dapr.PublishEventAsync(PUBSUB_NAME, TOPIC_NAME, new { order.Id, order.Total, order.BuyerId, order.Address });

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<Pageable<OrderDto>> SearchAsync(OrderQuerySearch querySearch)
        {
            var query = _applicationContext.GetQuery<Order>(querySearch).ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var totalItem = query.Count();

            var items = await query.ApplyPaging(querySearch.GetSkip(), querySearch.GetTake()).ToListAsync();

            return new Pageable<OrderDto>(totalItem, querySearch.GetTake(), querySearch.PageIndex, items);
        }

        public async Task DeleteBasketAsync(string id, CancellationToken cancellationToken = default)
        {
            await _dapr.DeleteStateAsync(STORE_NAME, id, cancellationToken: cancellationToken);
        }

        public async Task<CustomerBasketDto> GetBasketAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var result = await _dapr.GetStateAsync<CustomerBasketDto>(STORE_NAME, customerId, cancellationToken: cancellationToken);

            return result;
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket, CancellationToken cancellationToken = default)
        {
            var state = await _dapr.GetStateEntryAsync<CustomerBasketDto>(STORE_NAME, basket.BuyerId, cancellationToken: cancellationToken);

            state.Value = basket;

            await state.SaveAsync(cancellationToken: cancellationToken);

            var result = await GetBasketAsync(basket.BuyerId, cancellationToken);

            return result;
        }

        public async Task<OrderDto> GetByIdAsync(string id)
        {
            var order = await _applicationContext.GetDbSet<Order>().Include(x => x.Items).SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (order == null) throw new NotFoundException(id);

            return _mapper.Map<OrderDto>(order);
        }
    }
}
