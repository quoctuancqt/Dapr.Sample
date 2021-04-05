using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Extensions;
using SharedKernel.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SharedKernel
{
    public abstract class ApplicationContext : DbContext, IApplicationContext, IDisposable
    {
        private readonly HttpContext _httpContext;
        private string UserId { get; set; }

        protected ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationContext(DbContextOptions options,
            IHttpContextAccessor _httpContextAccessor) : base(options)
        {
            _httpContext = _httpContextAccessor?.HttpContext;

            if (_httpContext != null && _httpContext.User != null && _httpContext.User.Claims != null)
            {
                UserId = _httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }
        }
        public virtual async Task CommitAsync(bool isAudits = true)
        {
            BeforeCommit(isAudits);

            await SaveChangesAsync();
        }

        public virtual async Task CommitAsync(Func<Task> action)
        {
            BeforeCommit();

            var strategy = Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = Database.BeginTransaction();

                await SaveChangesAsync();

                await action();

                transaction.Commit();
            });
        }

        public virtual async Task ExecuteSqlRawAsync(string query)
        {
            await ExecuteSqlRawAsync(query);
        }

        public DbSet<T> GetDbSet<T>() where T : BaseEntity, IBaseEntity
        {
            return Set<T>();
        }

        public IQueryable<T> GetQuery<T>(BaseQuery querySearch) where T : BaseEntity, IBaseEntity
        {
            var query = Set<T>().ApplyLikeSearch(querySearch.SearchText, querySearch.SearchFields);

            if (querySearch.OrderBy.Any())
            {
                query = query.ApplySort(querySearch.OrderBy);
            }
            else
            {
                query = query.ApplySort(new string[] { "createdAt" });
            };

            return query;
        }

        private void BeforeCommit(bool isAudits = true)
        {
            var entriesAdded = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            var entriesModified = ChangeTracker.Entries()
                  .Where(e => e.State == EntityState.Modified).Select(e => e.Entity as IAudit);

            if (entriesAdded.Any()) ProcessAudit(entriesAdded, EntityState.Added);

            if (entriesModified.Any()) ProcessAudit(entriesModified, EntityState.Modified);

        }

        private void ProcessAudit(IEnumerable<object> entries, EntityState state)
        {
            foreach (var e in entries.Select(e => e as IAudit))
            {
                if (e != null)
                {
                    if (state == EntityState.Added)
                    {
                        e.CreatedBy = UserId;

                        e.CreatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        e.UpdatedBy = UserId;

                        e.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
