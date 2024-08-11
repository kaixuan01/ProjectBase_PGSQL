using DAL;
using DAL.Entity;
using DBL.User_Service.UserService;
using DBL.User_Service.UserService.VerifyUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Utils.Tools;
namespace E_commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : BaseAPIController
    {
        private readonly IUserService _userService;

        public OAuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = "GetOAuth")]
        public async Task<IActionResult> Get([FromQuery] VerifyUser_REQ user)
        {
            if (!string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(user.password))
            {
                var _authToken = new AuthToken();
                var success = await _userService.VerifyUserAsync(user);
                if (success)
                {
                    var token = _authToken.GenerateJwtToken(user.username);

                    Response.Cookies.Append("authToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

    }
}
