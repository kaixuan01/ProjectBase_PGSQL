using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserLoginHistoryService;
using Microsoft.AspNetCore.Mvc;
using Utils.Model;

namespace E_commerce.Controllers.User_Controller
{
    public class UserLoginHistoryController : BaseAPIController
    {
        private readonly IUserLoginHistoryService _userLoginHistoryService;

        public UserLoginHistoryController(IUserLoginHistoryService userLoginHistoryService)
        {
            _userLoginHistoryService = userLoginHistoryService;
        }

        [HttpGet]
        [Route("GerUserLoginHistoryList")]
        public async Task<IActionResult> GetUserLoginHistoryList([FromQuery] FilterParameters filterParameters)
        {
            ApiResponse<PagedResult<T_UserLoginHistory>>? apiResponse = null;

            try
            {
                var result = await _userLoginHistoryService.GetLoginHistoryList(filterParameters, false);

                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<PagedResult<T_UserLoginHistory>>.CreateSuccessResponse(result, "Get T_User Login History List Successful");
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<PagedResult<T_UserLoginHistory>>.CreateErrorResponse($"Get T_User Login History List Failed. Exception: {ex.Message}");
            }


            return Ok(apiResponse);
        }
    }
}
