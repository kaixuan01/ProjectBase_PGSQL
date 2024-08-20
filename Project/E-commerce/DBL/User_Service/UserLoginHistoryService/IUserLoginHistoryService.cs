using DAL.Entity;
using DAL.Shared.Class;
using DAL.Tools.ListingHelper;

namespace DBL.User_Service.UserLoginHistoryService
{
    public interface IUserLoginHistoryService
    {
        Task<ShareResp> CreateAsync(T_UserLoginHistory userLoginHistory);

        Task<PagedResult<T_UserLoginHistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false);

        Task UpdateUserLogoutByUserIdAsync(string UserId);

    }
}
