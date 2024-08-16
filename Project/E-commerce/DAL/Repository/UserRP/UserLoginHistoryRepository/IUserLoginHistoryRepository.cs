using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public interface IUserLoginHistoryRepository : IListingHelper<UserLoginHistory>
    {
        Task<List<UserLoginHistory>> GetAllAsync();
        Task<UserLoginHistory> CreateAsync(UserLoginHistory user);
    }
}
