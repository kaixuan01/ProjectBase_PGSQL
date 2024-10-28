using DAL.Models;

namespace DAL.Repository.UserRP.UserTokens
{
    public interface IUserTokensRepository
    {
        Task<TUsertoken> GetByTokenAsync(string token);
        Task<TUsertoken> GetByUserIdAsync(string UserId);
        Task CreateAsync(TUsertoken userTokens);
        Task UpdateAsync(TUsertoken userTokens);
    }
}
