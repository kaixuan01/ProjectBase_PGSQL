using DAL.Repository.UserRP.UserRepository.Class;
using DBL.User_Service.UserRoleService;
using DBL.User_Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils.Enums;
using Utils.Model;
using Utils;
using E_commerce.Tools;
using DAL.Entity;

namespace E_commerce.Controllers.User_Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController : BaseAPIController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        [Route("GetRoleList")]
        public async Task<IActionResult> GetRoleList()
        {
            ApiResponse<List<E_UserRole>>? apiResponse = null;
            LogHelper.FormatMainLogMessage(Enum_LogLevel.Information, $"Receive Request Get User Role List.");

            try
            {
                var oResp = await _userRoleService.GetUserRoleListingAsync();
                apiResponse = ApiResponse<List<E_UserRole>>.CreateSuccessResponse(oResp, "Get User Role List Successful");
            }
            catch (Exception ex)
            {
                LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"Exception when Get User Role List, Message: {ex.Message}", ex);

                apiResponse = ApiResponse<List<E_UserRole>>.CreateErrorResponse($"Get User Role List Failed. Exception: {ex.Message}");
            }

            return Ok(apiResponse);
        }
    }
}
