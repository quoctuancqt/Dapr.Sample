using System.Linq;
using System.Linq.Expressions;

namespace SharedKernel.Extensions
{
    public static class QueryProviderExtensions
    {
        public static IOrderedQueryable CreateOrderedQuery(this IQueryProvider provider, MethodCallExpression call)
        {
            return (IOrderedQueryable)provider.CreateQuery(call);
        }

        public static IOrderedQueryable<T> CreateOrderedQuery<T>(this IQueryProvider provider, MethodCallExpression call)
        {
            return (IOrderedQueryable<T>)provider.CreateQuery(call);
        }
    }
}
