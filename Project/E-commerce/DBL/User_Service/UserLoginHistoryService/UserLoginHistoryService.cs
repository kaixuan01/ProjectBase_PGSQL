using DAL.Entity;
using DAL.Repository.UserRP.UserLoginHistoryRepository;
using DAL.Tools.ListingHelper;

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

        public async Task<PagedResult<UserLoginHistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false)
        {
            var oUserLoginHistoryList = await _userLoginHistoryRepository.GetPagedListAsync(filterParameters, includeForeignRelationship);


            return oUserLoginHistoryList;
        }
    }
}
