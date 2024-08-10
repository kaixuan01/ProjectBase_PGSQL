using DAL;
using DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Utils.Tools;
namespace E_commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : ControllerBase
    {


        [HttpGet(Name = "GetOAuth")]
        public string Get(User userInfo)
        {
            if(string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Password))
            {
                var _authToken = new AuthToken();
                if (true)//sucess authorization
                {
                    return _authToken.GenerateJwtToken(userInfo.UserName);
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
