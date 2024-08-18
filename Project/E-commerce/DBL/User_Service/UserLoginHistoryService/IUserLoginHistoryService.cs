using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.Shared.Class;

namespace DBL.User_Service.UserLoginHistoryService
{
    public interface IUserLoginHistoryService
    {
        Task<List<T_UserLoginHistory>> GetAllUserLoginHistoryAsync();

        Task<ShareResp> CreateAsync(T_UserLoginHistory userLoginHistory);

        Task<PagedResult<T_UserLoginHistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false);


    }
}
