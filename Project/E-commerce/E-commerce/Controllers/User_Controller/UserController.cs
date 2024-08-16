using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService;
using DBL.User_Service.UserService.UserActionClass;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Utils.Model;

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

            try
            {
                var result = await _userService.GetPagedListAsync(filterParameters);
                
                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<PagedResult<dynamic>>.CreateSuccessResponse(result, "Get User List Successful");
            }
            catch (Exception ex)
            {
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
            }


            return Ok(apiResponse);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            ApiResponse<string>? apiResponse = null;

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
                apiResponse = ApiResponse<String>.CreateErrorResponse($"Delete User Failed. Exception: {ex.Message}");
            }


            return Ok(apiResponse);
        }
    }
}
