using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public static class Extensions
    {

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
         this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
         where T : class
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            var totalCount = source.Count();

            var items = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };

        }
    
  
    }
}
