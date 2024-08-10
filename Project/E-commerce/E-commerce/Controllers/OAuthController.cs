using DAL;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.Tools;
namespace E_commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : BaseAPIController
    {
        [AllowAnonymous]
        [HttpGet(Name = "GetOAuth")]
        public string Get(string username, string password)
        {
            if(!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                var _authToken = new AuthToken();
                if (true)//sucess authorization
                {
                    return _authToken.GenerateJwtToken(username);
                }
                else
                {
                    return "";
                }
            }
            return "";

        }
    }
}
