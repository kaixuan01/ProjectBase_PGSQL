using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public interface IUserLoginHistoryRepository
    {
        Task<List<UserLoginHistory>> GetAllAsync();
        Task<UserLoginHistory> CreateAsync(UserLoginHistory user);
    }
}
