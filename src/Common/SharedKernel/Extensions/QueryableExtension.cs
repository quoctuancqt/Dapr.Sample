using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using SharedKernel.Enums;

namespace SharedKernel.Extensions
{
    public static class QueryableExtensions
    {
        private static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static IQueryable<TResult> ApplySort<TResult>(
            this IQueryable<TResult> query,
            string sortField = "createdAt",
            OrderingDirection sortType = OrderingDirection.Desc) where TResult : class
        {
            query = sortType == OrderingDirection.Asc ? query.OrderBy(sortField.FirstCharToUpper()) : query.OrderByDescending(sortField.FirstCharToUpper());

            return query;
        }

        public static IQueryable<TResult> ApplySort<TResult>(
            this IQueryable<TResult> query,
            string[] sortFields,
            OrderingDirection sortType = OrderingDirection.Desc) where TResult : class
        {
            if (sortFields == null || !sortFields.Any()) return query;

            var orderedQuery = query.OrderBy(x => true);

            foreach (var sortField in sortFields.Select((value, index) => new { Value = value, Index = index }))
            {
                var expression = OrderByExtension.GetExpression<TResult>(sortField.Value);

                if (sortType == OrderingDirection.Asc)
                {
                    orderedQuery = sortField.Index == 0 ? orderedQuery.OrderBy(expression) : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = sortField.Index == 0 ? orderedQuery.OrderByDescending(expression) : orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;

            return query;
        }

        public static IQueryable<TResult> ApplyLikeSearch<TResult>(
            this IQueryable<TResult> query,
            string searchKey,
            params string[] searchFields)
        {
            if (string.IsNullOrEmpty(searchKey)) return query;

            Expression<Func<TResult, bool>> expression = null;
            var parameterExpression = Expression.Parameter(typeof(TResult), "p");

            var pattern = Expression.Constant($"%{searchKey}%");

            foreach (var searchField in searchFields)
            {
                Expression<Func<TResult, bool>> lambda;
                var nestedProperties = searchField.Split('.');
                Expression member = parameterExpression;

                foreach (var prop in nestedProperties)
                {
                    member = Expression.PropertyOrField(member, prop);
                }

                if (member.Type.Name.Equals("String"))
                {
                    var likeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
                        new[]
                        {
                            typeof(DbFunctions),
                            typeof(string),
                            typeof(string)
                        });
                    Expression call = Expression.Call(null, likeMethod, Expression.Constant(EF.Functions), member, pattern);

                    lambda = Expression.Lambda<Func<TResult, bool>>(call, parameterExpression);
                }
                else
                {
                    lambda = Expression.Lambda<Func<TResult, bool>>(Expression.Equal(member, Expression.Constant(searchKey)), parameterExpression);
                }

                expression = expression == null ? lambda : expression.Or(lambda);
            }

            if (expression == null) return query;

            query = query.Where(expression);

            return query;
        }

        public static IQueryable<TResult> ApplyEnumsFilter<TResult, TEnum>(
            this IQueryable<TResult> query,
            IList<TEnum> statuses,
            string filteredField)
        {
            if (!statuses.Any()) return query;

            Expression<Func<TResult, bool>> expression = null;
            var parameterExpression = Expression.Parameter(typeof(TResult), "p");

            Expression member = parameterExpression;
            member = Expression.PropertyOrField(member, filteredField);

            foreach (var status in statuses)
            {
                var lambda = Expression.Lambda<Func<TResult, bool>>(Expression.Equal(member, Expression.Constant(status)), parameterExpression);
                expression = expression == null ? lambda : expression.Or(lambda);
            }

            if (expression == null) return query;

            query = query.Where(expression);

            return query;
        }

        public static IQueryable<TResult> ApplyFilterValues<TResult>(
            this IQueryable<TResult> query,
            string values,
            string filteredField)
        {
            if (string.IsNullOrEmpty(values)) return query;

            Expression<Func<TResult, bool>> expression = null;
            var parameterExpression = Expression.Parameter(typeof(TResult), "p");

            Expression member = parameterExpression;
            member = Expression.PropertyOrField(member, filteredField);

            var items = values.Split(",");
            foreach (var item in items)
            {
                var equalMethod = typeof(string).GetMethod("Equals",
                    new[]
                    {
                        typeof(string),
                        typeof(string)
                    });

                Expression call = Expression.Call(null, equalMethod, Expression.Constant(item), member);
                var lambda = Expression.Lambda<Func<TResult, bool>>(call, parameterExpression);

                expression = expression == null ? lambda : expression.Or(lambda);
            }

            if (expression == null) return query;

            query = query.Where(expression);

            return query;
        }

        public static IQueryable<TResult> ApplyFilterValues<TResult>(
            this IQueryable<TResult> query,
            string[] values,
            string filteredField)
        {
            if (values == null || !values.Any()) return query;

            Expression<Func<TResult, bool>> expression = null;
            var parameterExpression = Expression.Parameter(typeof(TResult), "p");

            Expression member = parameterExpression;
            member = Expression.PropertyOrField(member, filteredField);

            foreach (var item in values)
            {
                var equalMethod = typeof(string).GetMethod("Equals",
                    new[]
                    {
                        typeof(string),
                        typeof(string)
                    });

                Expression call = Expression.Call(null, equalMethod, Expression.Constant(item), member);
                var lambda = Expression.Lambda<Func<TResult, bool>>(call, parameterExpression);

                expression = expression == null ? lambda : expression.Or(lambda);
            }

            if (expression == null) return query;

            query = query.Where(expression);

            return query;
        }

        public static IQueryable<TResult> ApplyPaging<TResult>(this IQueryable<TResult> query, int skip, int take)
        {
            return query.Skip(skip).Take(take);
        }
    }
}
