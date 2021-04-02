using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Intefaces
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(string id) where T : BaseEntity, IBaseEntity;
        Task<List<T>> ListAsync<T>() where T : BaseEntity, IBaseEntity;
        Task<List<T>> ListAsync<T>(ISpecification<T> spec) where T : BaseEntity, IBaseEntity;
        Task<T> AddAsync<T>(T entity) where T : BaseEntity, IBaseEntity;
        Task UpdateAsync<T>(T entity) where T : BaseEntity, IBaseEntity;
        Task DeleteAsync<T>(T entity) where T : BaseEntity, IBaseEntity;
    }
}
