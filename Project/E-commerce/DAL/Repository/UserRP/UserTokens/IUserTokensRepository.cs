using DAL.Entity;

namespace DAL.Repository.UserRP.UserTokens
{
    public interface IUserTokensRepository
    {
        Task<T_UserTokens> GetByTokenAsync(string token);
        Task<T_UserTokens> GetByUserIdAsync(string UserId);
        Task CreateAsync(T_UserTokens userTokens);
        Task UpdateAsync(T_UserTokens userTokens);
    }
}
