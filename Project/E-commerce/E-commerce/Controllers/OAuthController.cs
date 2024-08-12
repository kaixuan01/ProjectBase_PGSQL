using DAL;
using DAL.Entity;
using DBL.User_Service.UserService;
using DBL.User_Service.UserService.VerifyUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Utils.Tools;
using Utils.Model;
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

        [HttpPost(Name = "OAuth")]
        public async Task<IActionResult> OAuth([FromBody] VerifyUser_REQ user)
        {
            if (!string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(user.password))
            {
                var _authToken = new AuthToken();
                var success = await _userService.VerifyUserAsync(user);
                if (success)
                {
                    var token = _authToken.GenerateJwtToken(user.username);

                    // Set the token as an HttpOnly, Secure cookie
                    Response.Cookies.Append("authToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    // Create a success response using ApiResponse<T>
                    var response = ApiResponse<string>.CreateSuccessResponse(null, "Login successful");

                    return Ok(response);
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
