using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class ExtensionFunctions
    {
        public static string ToISOString(this DateTime dt)
        {
            if (dt != null)
            {
                return dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else { return null; }
        }

        //public static DateTime? FromISOString(this string? dtstr,string? st=null )
        //{     
        //    return null;
        //}

        public static DateTime FromISOString(this string dtstr)
        {
            return DateTime.ParseExact(dtstr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static async Task<List<T>> PageDataAsync<T>(this IQueryable<T> query, int PageSize = 0, int PageNumber = 1)
        {
            return await (PageSize == 0 ? query.ToListAsync() : query.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync());
        }

        public static List<T> PageData<T>(this IQueryable<T> query, int PageSize = 0, int PageNumber = 1)
        {
            return (PageSize == 0 ? query.ToList() : query.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList());
        }

    }
}
