using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.SystemConfigRP
{
    public class SystemConfigRepository :  ISystemConfigRepository
    {
        private readonly AppDbContext _appDbContext;

        public SystemConfigRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task UpdateAsync(TSystemconfig config)
        {
            // Attach the user entity to the context
            _appDbContext.Attach(config);

            // Mark all properties as modified
            _appDbContext.Entry(config).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TSystemconfig> GetByKeyAsync(string key)
        {
            return await _appDbContext.TSystemconfigs.FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task<List<TSystemconfig>> GetAllAsync()
        {
            return await _appDbContext.TSystemconfigs.ToListAsync();
        }
    }
}
