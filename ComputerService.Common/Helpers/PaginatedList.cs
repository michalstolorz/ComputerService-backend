using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ComputerService.Common.Helpers
{
    public class PaginatedList<T>
    {
        public Pagination Pagination { get; set; }
        public List<T> Items { get; set; }

        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Pagination = new Pagination(pageIndex, pageSize, totalCount);
            Items = items;
        }

        public PaginatedList(List<T> items, Pagination pagination)
        {
            Pagination = pagination;
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source is IAsyncEnumerable<T>
                ? await source.CountAsync()
                : source.Count();

            var getQuery = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var items = source is IAsyncEnumerable<T>
                ? await getQuery.ToListAsync()
                : getQuery.ToList();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
