using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Models;
using RoleBasedAuthorization.Service;


namespace RoleBasedAuthorization.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {

        private IAuthenticateService _content;
        public TokenController(IAuthenticateService conetext)
        {
            _content = conetext;
        }


        //login
        [AllowAnonymous]
        [HttpPost("login")]
        public JsonResult Login([FromBody] AuthenticateModel login)
        {
            //check user login
            var user = _content.Login(login.username, login.password);

            if (user == null)

                return Json(JsonResultResponse.ResponseFail("Username or password incorrect."));

            return Json(JsonResultResponse.ResponseSuccess(user));
        }
    }
}
