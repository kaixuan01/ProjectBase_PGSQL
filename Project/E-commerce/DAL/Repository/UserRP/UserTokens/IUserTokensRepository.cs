using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.UserRP.UserTokens
{
    public interface IUserTokensRepository
    {
        Task<T_UserTokens> GetByTokenAsync(string token);
        Task CreateAsync(T_UserTokens userTokens);
        Task UpdateAsync(T_UserTokens userTokens);
    }
}
