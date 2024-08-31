using DAL.Entity;

namespace DBL.User_Service.UserTokensService
{
    public interface IUserTokensService
    {
        Task<T_UserTokens> GetByTokenAsync(string token);
        Task<T_UserTokens> GetByUserIdAsync(string UserId);
        Task<T_UserTokens> CreateAsync(string UserId, string TokenType);
        Task UpdateAsync(T_UserTokens userTokens);
    }
}
