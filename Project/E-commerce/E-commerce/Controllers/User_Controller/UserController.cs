using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService;
using DBL.User_Service.UserService.UserActionClass;
using E_commerce.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils;
using Utils.Enums;
using Utils.Model;
using Utils.Tools;

namespace E_commerce.Controllers.User_Controller
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseAPIController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList([FromQuery] FilterParameters filterParameters)
        {
            ApiResponse<PagedResult<dynamic>>? apiResponse = null;
            LogHelper.FormatMainLogMessage(LogLevelEnums.Information, $"Receive Request Get User List, FilterParameters: {JsonConvert.SerializeObject(filterParameters)}");

            try
            {
                var result = await _userService.GetPagedListAsync(filterParameters);

                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<PagedResult<dynamic>>.CreateSuccessResponse(result, "Get User List Successful");
            }
            catch (Exception ex)
            {
                LogHelper.FormatMainLogMessage(LogLevelEnums.Error, $"Exception when Get User List, Message: {ex.Message}", ex);

                apiResponse = ApiResponse<PagedResult<dynamic>>.CreateErrorResponse($"Get User List Failed. Exception: {ex.Message}");
            }

            return Ok(apiResponse);
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] CreateUser_REQ oUser)
        {
            ApiResponse<string>? apiResponse = null;

            try
            {
                if (!string.IsNullOrEmpty(oUser.password))
                {
                    oUser.password = PasswordHelper.HashPassword(oUser.password);
                }

                LogHelper.FormatMainLogMessage(LogLevelEnums.Information, $"Receive Request to add user, Request: {JsonConvert.SerializeObject(oUser)}");

                var oResp = await _userService.CreateAsync(oUser);

                switch (oResp.Code)
                {
                    case RespCode.RespCode_Success:
                        // Create a success response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateSuccessResponse(oResp.UserId, oResp.Message);
                        break;

                    case RespCode.RespCode_Failed:
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                    default: // Default is throw exception message
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                        // return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<String>.CreateErrorResponse($"Create User Failed. Exception: {ex.Message}");

                LogHelper.FormatMainLogMessage(LogLevelEnums.Error, $"Exception when create user, Message: {ex.Message}", ex);
            }


            return Ok(apiResponse);
        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] EditUser_REQ oUser)
        {
            ApiResponse<string>? apiResponse = null;

            try
            {
                if (!string.IsNullOrEmpty(oUser.password))
                {
                    oUser.password = PasswordHelper.HashPassword(oUser.password);
                }

                LogHelper.FormatMainLogMessage(LogLevelEnums.Information, $"Receive Request to edit user, Request: {JsonConvert.SerializeObject(oUser)}");

                var oResp = await _userService.UpdateAsync(oUser);

                switch (oResp.Code)
                {
                    case RespCode.RespCode_Success:
                        // Create a success response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateSuccessResponse(oResp.UserId, oResp.Message);
                        break;

                    case RespCode.RespCode_Failed:
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                    default: // Default is throw exception message
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                        // return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<String>.CreateErrorResponse($"Edit User Failed. Exception: {ex.Message}");

                LogHelper.FormatMainLogMessage(LogLevelEnums.Error, $"Exception when edit user, Message: {ex.Message}", ex);
            }


            return Ok(apiResponse);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            ApiResponse<string>? apiResponse = null;

            LogHelper.FormatMainLogMessage(LogLevelEnums.Information, $"Receive Request to delete user, userId: {userId}");

            try
            {
                var oResp = await _userService.DeleteAsync(userId);

                switch (oResp.Code)
                {
                    case RespCode.RespCode_Success:
                        // Create a success response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateSuccessResponse(null, oResp.Message);
                        break;

                    case RespCode.RespCode_Failed:
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                    default: // Default is throw exception
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                        // return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                LogHelper.FormatMainLogMessage(LogLevelEnums.Error, $"Exception when Delete, Message: {ex.Message}", ex);

                apiResponse = ApiResponse<String>.CreateErrorResponse($"Delete User Failed. Exception: {ex.Message}");
            }


            return Ok(apiResponse);
        }
    }
}
