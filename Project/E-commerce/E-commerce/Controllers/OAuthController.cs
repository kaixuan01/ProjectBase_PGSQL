using DAL.Repository.UserRP.UserRepository.Class;
using DBL.User_Service.UserService;
using DBL.User_Service.UserService.UserActionClass;
using E_commerce.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly int _expireMins;

        public OAuthController(IUserService userService, AuthToken authToken, IConfiguration configuration)
        {
            _userService = userService;
            _authToken = authToken;

            var jwtSettings = configuration.GetSection("JwtSettings");
            _expireMins = int.Parse(jwtSettings["ExpireMins"]);
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
                            var refreshToken = AuthToken.GenerateRefreshToken();
                            // Update the refresh token in your storage
                            AuthToken.StoreRefreshToken(user.username, refreshToken); // Implement StoreRefreshToken

                            // Set the tokens in cookies
                            SetCookies(token, refreshToken);

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

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                ClearCookies();

                // Retrieve the username from HttpContext.Items
                if (HttpContext.Items.TryGetValue("Username", out var username))
                {
                    // Use the username
                    string usernameStr = username as string;

                    LogHelper.FormatMainLogMessage(Enum_LogLevel.Information, $"Update User Login History, Username: {username}");

                    // Update user logout
                    await _userService.UpdateUserLogoutAsync(usernameStr);
                }

                var response = ApiResponse<string>.CreateSuccessResponse(null, "Logout successful");

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.CreateErrorResponse($"Logout failed, Exception: {ex.Message}");
                LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"Exception when logout, Exception: {ex.Message}");

                return Ok(response);
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            ApiResponse<string>? apiResponse = null;

            try
            {
                var authToken = Request.Cookies["authToken"];
                if (string.IsNullOrEmpty(authToken))
                {
                    ClearCookies();
                    LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"JWT Token not found");
                    return Unauthorized("JWT token not found.");
                }

                var username = User.FindFirstValue(ClaimTypes.Name);
                if (string.IsNullOrEmpty(username))
                {
                    ClearCookies();
                    LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"Username not found by the JWT tokent");
                    return Unauthorized("User not found.");
                }

                var storedRefreshToken = AuthToken.GetRefreshToken(username);

                // Retrieve the refresh token from the cookie
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(storedRefreshToken))
                {
                    ClearCookies();
                    LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"Refresh Token not found. Cookies refresh token: {refreshToken}, Stored Refresh Token: {storedRefreshToken}");
                    return Unauthorized("Refresh token not found.");
                }

                if (storedRefreshToken == refreshToken)
                {
                    var userRole = await _userService.GetUserRoleByUsernameAsync(username);
                    var newToken = _authToken.GenerateJwtToken(username, (Enum_UserRole)userRole);
                    var newRefreshToken = AuthToken.GenerateRefreshToken();

                    // Update stored refresh token
                    AuthToken.StoreRefreshToken(username, newRefreshToken);

                    // Set the new tokens in cookies
                    SetCookies(newToken, newRefreshToken);

                    apiResponse = ApiResponse<string>.CreateSuccessResponse(null, "Refresh token successful");
                }
                else
                {
                    ClearCookies();
                    LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"Cookies refresh token not equal with localstorage. Cookies refresh token: {refreshToken}, Stored Refresh Token: {storedRefreshToken}");
                    return Unauthorized("Wrong Refresh Token.");
                }
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<string>.CreateErrorResponse($"Refresh Token Failed. Exception: {ex.Message}");
                LogHelper.FormatMainLogMessage(Enum_LogLevel.Error, $"Exception when refresh token, Message: {ex.Message}", ex);
            }

            return Ok(apiResponse);
        }

        #region [ Function ]

        private void ClearCookies()
        {
            // Clear cookies by setting an expiration date in the past
            Response.Cookies.Append("authToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // Specify SameSite for cross-site context
                Expires = DateTime.UtcNow.AddDays(-1) // Set expiry date to past to delete the cookie
            });

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // Specify SameSite for cross-site context
                Expires = DateTime.UtcNow.AddDays(-1) // Set expiry date to past to delete the cookie
            });
        }

        private void SetCookies(string authToken, string refreshToken)
        {
            // var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            Response.Cookies.Append("authToken", authToken, new CookieOptions
            {
                HttpOnly = true, // Prevent access via JavaScript
                Secure = true,   // Ensure the cookie is sent only over HTTPS
                SameSite = SameSiteMode.None, // Set none
                Expires = DateTime.UtcNow.AddMinutes(_expireMins),
            });

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(_expireMins),
            });
        }

        #endregion
    }
}
