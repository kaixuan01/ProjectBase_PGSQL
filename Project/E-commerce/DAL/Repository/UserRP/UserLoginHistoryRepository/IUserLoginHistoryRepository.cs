using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public interface IUserLoginHistoryRepository : IListingHelper<T_UserLoginHistory>
    {
        Task CreateAsync(T_UserLoginHistory user);
        Task UpdateAsync(T_UserLoginHistory oRec);
        Task<T_UserLoginHistory> GetUserLoginHistoryByUserIdAsync(string UserId);

    }
}
