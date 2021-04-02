using Microsoft.EntityFrameworkCore;
using SharedKernel.Intefaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel
{
    public class BaseRepository : IRepository
    {
        private readonly ApplicationContext _dbContext;

        public BaseRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(string id) where T : BaseEntity, IBaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id.Equals(id));
        }

        public Task<T> GetByIdAsync<T>(string id) where T : BaseEntity, IBaseEntity
        {
            return _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public Task<List<T>> ListAsync<T>() where T : BaseEntity, IBaseEntity
        {
            return _dbContext.Set<T>().ToListAsync();
        }

        public Task<List<T>> ListAsync<T>(ISpecification<T> spec) where T : BaseEntity, IBaseEntity
        {
            var specificationResult = ApplySpecification(spec);
            return specificationResult.ToListAsync();
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity, IBaseEntity
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.CommitAsync();

            return entity;
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity, IBaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.CommitAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity, IBaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.CommitAsync();
        }

        private IQueryable<T> ApplySpecification<T>(ISpecification<T> spec) where T : BaseEntity
        {
            var evaluator = new SpecificationEvaluator<T>();
            return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}