using DBL.User_Service.UserService;
using DBL.User_Service.UserService.UserActionClass;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Utils.Enums;
using Utils.Model;
using Utils.Tools;
namespace E_commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : BaseAPIController
    {
        private readonly IUserService _userService;
        private readonly AuthToken _authToken;
        private readonly int _expireHours;

        public OAuthController(IUserService userService, AuthToken authToken, IConfiguration configuration)
        {
            _userService = userService;
            _authToken = authToken;

            var jwtSettings = configuration.GetSection("JwtSettings");
            _expireHours = int.Parse(jwtSettings["ExpireHours"]);
        }

        [HttpPost(Name = "OAuth")]
        public async Task<IActionResult> OAuth([FromBody] VerifyUser_REQ user)
        {
            if (!string.IsNullOrEmpty(user.username) && !string.IsNullOrEmpty(user.password))
            {
                var oVerifyResp = await _userService.VerifyUserAsync(user);

                if (oVerifyResp != null)
                {
                    switch (oVerifyResp.Code)
                    {
                        case RespCode.RespCode_Success:
                            var token = _authToken.GenerateJwtToken(user.username, (Enum_UserRole)oVerifyResp.UserRoleId);

                            Response.Cookies.Append("authToken", token, new CookieOptions
                            {
                                HttpOnly = true, // Prevent access via JavaScript
                                Secure = true,   // Ensure the cookie is sent only over HTTPS
                                Expires = DateTime.UtcNow.AddHours(_expireHours),
                                SameSite = SameSiteMode.Strict // Prevent CSRF attacks
                            });

                            var response = ApiResponse<string>.CreateSuccessResponse(null, "Login successful");
                            return Ok(response);

                        case RespCode.RespCode_Failed:
                            var errorResponse = ApiResponse<string>.CreateErrorResponse(oVerifyResp.Message);
                            return Ok(errorResponse);

                        default:
                            var exceptionResponse = ApiResponse<string>.CreateErrorResponse(oVerifyResp.Message);

                            return Ok(exceptionResponse);
                            // return Unauthorized();
                    }
                }
            }
            return Ok(ApiResponse<string>.CreateErrorResponse("Username or password cannot be empty"));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("authToken");
            return Ok(new { Message = "Logout successful" });
        }

    }
}
