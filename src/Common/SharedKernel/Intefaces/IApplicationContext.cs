using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Intefaces
{
    public interface IApplicationContext : IDisposable
    {
        DatabaseFacade Database { get; }

        DbSet<T> GetDbSet<T>() where T : class, IBaseEntity;

        IQueryable<T> GetQuery<T>(BaseQuery querySearch) where T : class, IBaseEntity;

        Task ExecuteSqlRawAsync(string query);

        Task CommitAsync(bool isAudits = true);

        Task CommitAsync(Func<Task> action);
    }
}
