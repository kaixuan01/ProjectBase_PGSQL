using DAL.Entity;
using DAL.Repository.UserRP.UserLoginHistoryRepository;
using DAL.Tools.ListingHelper;
using DBL.Shared.Class;
using Utils;

namespace DBL.User_Service.UserLoginHistoryService
{
    public class UserLoginHistoryService : IUserLoginHistoryService
    {
        private readonly IUserLoginHistoryRepository _userLoginHistoryRepository;

        public UserLoginHistoryService(IUserLoginHistoryRepository userLoginHistoryRepository)
        {
            _userLoginHistoryRepository = userLoginHistoryRepository;
        }

        public async Task<List<T_UserLoginHistory>> GetAllUserLoginHistoryAsync()
        {
            var oUserLoginHistoryList = await _userLoginHistoryRepository.GetAllAsync();


            return oUserLoginHistoryList;
        }

        public async Task<ShareResp> CreateAsync(T_UserLoginHistory oUserLoginHistory)
        {
            var rtnValue = new ShareResp();

            try
            {
                await _userLoginHistoryRepository.CreateAsync(oUserLoginHistory);

                rtnValue.Code = RespCode.RespCode_Success;
                rtnValue.Message = RespCode.RespMessage_Insert_Successful;
            }
            catch (Exception ex)
            {

                rtnValue.Code = RespCode.RespCode_Exception;
                rtnValue.Message = ex.Message;
            }

            return rtnValue;
        }

        public async Task<PagedResult<T_UserLoginHistory>> GetLoginHistoryList(FilterParameters filterParameters, bool includeForeignRelationship = false)
        {
            var oUserLoginHistoryList = await _userLoginHistoryRepository.GetPagedListAsync(filterParameters, includeForeignRelationship);

            return oUserLoginHistoryList;
        }
    }
}
