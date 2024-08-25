using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.User_Service.UserTokensService
{
    public interface IUserTokensService
    {
        Task<T_UserTokens> GetByTokenAsync(string token);
        Task<T_UserTokens> CreateAsync(string UserId, string TokenType);
        Task UpdateAsync(T_UserTokens userTokens);
    }
}
