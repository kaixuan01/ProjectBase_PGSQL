using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.SystemConfigRP
{
    public class SystemConfigRepository :  ISystemConfigRepository
    {
        private readonly MyDbContext _myDbContext;

        public SystemConfigRepository(MyDbContext context)
        {
            _myDbContext = context;
        }

        public async Task UpdateAsync(T_SystemConfig config)
        {
            // Attach the user entity to the context
            _myDbContext.Attach(config);

            // Mark all properties as modified
            _myDbContext.Entry(config).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<T_SystemConfig> GetByKeyAsync(string key)
        {
            return await _myDbContext.T_SystemConfig.FirstOrDefaultAsync(c => c.Key == key);
        }

        public async Task<List<T_SystemConfig>> GetAllAsync()
        {
            return await _myDbContext.T_SystemConfig.ToListAsync();
        }
    }
}
