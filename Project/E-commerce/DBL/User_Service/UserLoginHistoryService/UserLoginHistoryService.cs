using DAL.Entity;
using DAL.Repository.UserRP.UserLoginHistoryRepository;

namespace DBL.User_Service.UserLoginHistoryService
{
    public class UserLoginHistoryService : IUserLoginHistoryService
    {
        private readonly IUserLoginHistoryRepository _userLoginHistoryRepository;

        public UserLoginHistoryService(IUserLoginHistoryRepository userLoginHistoryRepository)
        {
            _userLoginHistoryRepository = userLoginHistoryRepository;
        }

        public async Task<List<UserLoginHistory>> GetAllUserLoginHistoryAsync()
        {
            var oUserLoginHistoryList = await _userLoginHistoryRepository.GetAllAsync();


            return oUserLoginHistoryList;
        }

        public async Task<UserLoginHisotry_Create_RESP> CreateAsync(UserLoginHistory oUserLoginHistory)
        {
            var rtnValue = new UserLoginHisotry_Create_RESP();

            try
            {

                await _userLoginHistoryRepository.CreateAsync(oUserLoginHistory);

                rtnValue.Code = 0;
                rtnValue.Message = "Record created successful.";
            }
            catch (Exception ex)
            {

                rtnValue.Code = 9999;
                rtnValue.Message = ex.Message;
            }

            return rtnValue;
        }

    }
}
