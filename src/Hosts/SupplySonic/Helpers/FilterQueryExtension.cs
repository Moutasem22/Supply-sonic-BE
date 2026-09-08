using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

public static class FilterQueryExtension
{
    public static IQueryable<TSource> WhereByIf<TSource>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, bool>> predicate)
    {
        if (condition == true)
            return query.Where(predicate);
        else
            return query;
    }
}
