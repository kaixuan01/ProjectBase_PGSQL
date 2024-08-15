using DAL.Entity;

namespace DBL.User_Service.UserLoginHistoryService
{
    public interface IUserLoginHistoryService
    {
        Task<List<UserLoginHistory>> GetAllUserLoginHistoryAsync();

        Task<UserLoginHisotry_Create_RESP> CreateAsync(UserLoginHistory userLoginHistory);

    }
}
