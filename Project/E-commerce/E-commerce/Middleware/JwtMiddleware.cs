using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace E_commerce.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["authToken"];

            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
                var issuer = _configuration["JwtSettings:Issuer"];
                var audience = _configuration["JwtSettings:Audience"];

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = issuer,
                    ValidAudience = audience
                };

                try
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");

                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                    context.User = principal;

                    // ## Noted Use this method to get username
                    // var userName = User.FindFirstValue(ClaimTypes.Name);

                }
                catch
                {
                    // Token validation failed
                }
            }
            await _next(context);
        }
    }

}
