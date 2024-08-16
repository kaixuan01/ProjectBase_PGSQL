using DAL.Entity;
using DAL.Tools.ListingHelper;

namespace DBL.User_Service.UserLoginHistoryService
{
    public interface IUserLoginHistoryService
    {
        Task<List<UserLoginHistory>> GetAllUserLoginHistoryAsync();

        Task<UserLoginHisotry_Create_RESP> CreateAsync(UserLoginHistory userLoginHistory);

        Task<PagedResult<UserLoginHistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false);


    }
}
