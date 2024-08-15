using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService;
using Microsoft.AspNetCore.Mvc;
using Utils.Model;

namespace E_commerce.Controllers
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
        public async Task<IActionResult> GetUserList([FromQuery] FilterParameters filterParameters)
        {
            ApiResponse<PagedResult<User>>? apiResponse = null;

            try
            {
                var result = await _userService.GetPagedListAsync(filterParameters);

                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<PagedResult<User>>.CreateSuccessResponse(result, "Get User List Successful");
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<PagedResult<User>>.CreateErrorResponse($"Get User List Failed. Exception: {ex.Message}");
            }


            return Ok(apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User oUser)
        {
            ApiResponse<User>? apiResponse = null;

            try
            {
                var result = await _userService.CreateAsync(oUser);

                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<User>.CreateSuccessResponse(result, "Get User List Successful");
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<User>.CreateErrorResponse($"Get User List Failed. Exception: {ex.Message}");
            }


            return Ok(apiResponse);
        }
    }
}
