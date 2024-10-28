using DAL.Models;

namespace DAL.Repository.SystemConfigRP
{
    public interface ISystemConfigRepository
    {
        Task<TSystemconfig> GetByKeyAsync(string key);
        Task UpdateAsync(TSystemconfig systemConfig);
        Task<List<TSystemconfig>> GetAllAsync();
    }
}
