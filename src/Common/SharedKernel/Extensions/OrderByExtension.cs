using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedKernel.Extensions
{
    public static class OrderByExtension
    {
        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(object));
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        public static Func<TSource, object> GetFunc<TSource>(string propertyName)
        {
            return GetExpression<TSource>(propertyName).Compile();
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            return source.OrderBy(GetFunc<TSource>(propertyName));
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return source.OrderBy(GetExpression<TSource>(propertyName));
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            return source.OrderByDescending(GetFunc<TSource>(propertyName));
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return source.OrderByDescending(GetExpression<TSource>(propertyName));
        }

        private static IOrderedQueryable<T> ThenBy<T>(IOrderedQueryable<T> source, LambdaExpression expression)
        {
            return source.Provider.CreateOrderedQuery<T>(Expression.Call(typeof(Queryable),
                "ThenBy",
                new[] { GetElementType(source), expression.Body.Type },
                source.Expression,
                Expression.Quote(expression)));
        }

        private static IOrderedQueryable<T> ThenByDescending<T>(IOrderedQueryable<T> source, LambdaExpression expression)
        {
            return source.Provider.CreateOrderedQuery<T>(Expression.Call(typeof(Queryable),
                "ThenByDescending",
                new[] { GetElementType(source), expression.Body.Type },
                source.Expression,
                Expression.Quote(expression)));
        }

        private static Type GetElementType(IQueryable source)
        {
            var expr = source.Expression;
            var elementType = source.ElementType;
            while (expr.NodeType == ExpressionType.Call &&
                   elementType == typeof(object))
            {
                var call = (MethodCallExpression)expr;
                expr = call.Arguments.First();
                elementType = expr.Type.GetGenericArguments()
                    .First();
            }

            return elementType;
        }
    }
}
