using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Tools.ListingHelper
{
    public class ListingHelper<TEntity> : IListingHelper<TEntity> where TEntity : class
    {
        private readonly MyDbContext _context;

        public ListingHelper(MyDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TEntity>> GetPagedListAsync(FilterParameters parameters)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.SearchByFields(parameters.SearchTerm);
            }

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                // Apply sorting based on SortBy and SortDescending
                query = query.OrderByProperty(parameters.SortBy, parameters.SortDescending);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
