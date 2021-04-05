using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Dto;
using ProductService.Application.Entities;
using ProductService.Application.Interfaces;
using SharedKernel;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;
using SharedKernel.Intefaces;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IApplicationContext _applicationContext;
        private readonly IMapper _mapper;

        public ProductService(IApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetByIdAsync(string id)
        {
            var entity = await _applicationContext.GetDbSet<Product>().FindAsync(id);

            if (entity == null) throw new NotFoundException(id);

            return _mapper.Map<ProductDto>(entity);
        }

        public async Task<Pageable<ProductDto>> SearchAsync(ProductQuerySearch querySearch)
        {
            var query = _applicationContext.GetQuery<Product>(querySearch).ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            var totalItem = query.Count();

            var items = await query.ApplyPaging(querySearch.GetSkip(), querySearch.GetTake()).ToListAsync();

            return new Pageable<ProductDto>(totalItem, querySearch.GetTake(), querySearch.PageIndex, items);
        }
    }
}
