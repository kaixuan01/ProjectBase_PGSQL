using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utils.Enums;

namespace Utils.Tools
{
    public class AuthToken
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireHours;

        public AuthToken(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            _key = jwtSettings["Key"];
            _issuer = jwtSettings["Issuer"];
            _audience = jwtSettings["Audience"];
            _expireHours = int.Parse(jwtSettings["ExpireHours"]);
        }

        public string GenerateJwtToken(string username, Enum_UserRole? userRole)
        {
            if (userRole == null)
            {
                throw new Exception("User Role not found when generate JWT.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, userRole.ToString()) // Add user role as a claim
                }),
                Expires = DateTime.Now.AddHours(_expireHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer, // The issuer value from configuration
                Audience = _audience // The audience value from configuration
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
