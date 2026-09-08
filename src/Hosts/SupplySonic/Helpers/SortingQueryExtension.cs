using DTO;
using Elasticsearch.Net;
using Nest;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Helpers;

public static class SortingQueryExtension
{

    public static IOrderedQueryable<TSource> SortingByIf<TSource, TKey>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, TKey>> orderByExpression, string? sortType)
    {
        if (condition == true)
            return (SortTypeEnum)Convert.ToInt32(sortType) == SortTypeEnum.DESC ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
        else
            return (IOrderedQueryable<TSource>)query;
    }
}