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
                var oVerifyResp = await _userService.VerifyUserAsync(user);

                if (oVerifyResp != null)
                {
                    switch (oVerifyResp.Code)
                    {
                        case RespCode.RespCode_Success:
                            var token = _authToken.GenerateJwtToken(user.username);

                            Response.Cookies.Append("authToken", token, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTimeOffset.UtcNow.AddHours(1)
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

    }
}
