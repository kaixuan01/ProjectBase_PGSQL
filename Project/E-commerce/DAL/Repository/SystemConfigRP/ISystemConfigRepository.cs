using DAL.Entity;
using DAL.Tools.ListingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.SystemConfigRP
{
    public interface ISystemConfigRepository
    {
        Task<T_SystemConfig> GetByKeyAsync(string key);
        Task UpdateAsync(T_SystemConfig systemConfig);
        Task<List<T_SystemConfig>> GetAllAsync();
    }
}
