using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DAL.Repository.UserRP.UserLoginHistoryRepository
{
    public interface IUserLoginHistoryRepository : IListingHelper<T_UserLoginHistory>
    {
        Task<List<T_UserLoginHistory>> GetAllAsync();
        Task CreateAsync(T_UserLoginHistory user);
    }
}
