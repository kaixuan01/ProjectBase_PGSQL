using Microsoft.EntityFrameworkCore;

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

            if (!string.IsNullOrEmpty(parameters.SortBy) && parameters.SortDescending.HasValue)
            {
                // Apply sorting based on SortBy and SortDescending
                query = query.OrderByProperty(parameters.SortBy, (bool)parameters.SortDescending);
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
