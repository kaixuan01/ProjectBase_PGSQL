using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DBL.User_Service.UserLoginHistoryService
{
    public interface IUserLoginHistoryService
    {
        Task<List<T_UserLoginHistory>> GetAllUserLoginHistoryAsync();

        Task<UserLoginHisotry_Create_RESP> CreateAsync(T_UserLoginHistory userLoginHistory);

        Task<PagedResult<T_UserLoginHistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false);


    }
}
