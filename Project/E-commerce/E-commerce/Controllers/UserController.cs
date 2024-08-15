using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService;
using Microsoft.AspNetCore.Http;
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
            var result = await _userService.GetPagedListAsync(filterParameters);

            // Create a success response using ApiResponse<T>
            var response = ApiResponse<PagedResult<User>>.CreateSuccessResponse(result, "Get User List Successful");

            return Ok(response);
        }
    }
}
