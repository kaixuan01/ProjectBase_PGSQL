using DAL.Models;

namespace DBL.User_Service.UserTokensService
{
    public interface IUserTokensService
    {
        Task<TUsertoken> GetByTokenAsync(string token);
        Task<TUsertoken> GetByUserIdAsync(string UserId);
        Task<TUsertoken> CreateAsync(string UserId, string TokenType);
        Task UpdateAsync(TUsertoken userTokens);
    }
}
