using DAL.Tools.ListingHelper;
using DBL.User_Service.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetAllUserAsync();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] FilterParameters filterParameters)
        {
            var result = await _userService.GetPagedListAsync(filterParameters);
            return Ok(result);
        }
    }
}
