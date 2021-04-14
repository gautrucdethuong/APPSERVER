using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;


namespace RoleBasedAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSLoginController : Controller
    {

        private readonly ISMSLoginService _smssender;
        public SMSLoginController(ISMSLoginService context)
        {
            _smssender = context;
        }

        [HttpPost("smssender")]
        public JsonResult SenderOTP([FromBody] OTPModel model)
        {
            if (ModelState.IsValid)
            {
                if (_smssender.SMSSendOTPLogin(model.phone) == true)
                {
                    return Json(JsonResultResponse.ResponseChange("OTP sent successed."));
                }
            }
            return Json(JsonResultResponse.ResponseFail("OTP sent failed.")); 
        }


        [HttpPost("verifysms")]
        public JsonResult VerificationOTP([FromBody] VerifyModel model)
        {
            var user = _smssender.VerificationOTPLogin(model.otp);

            if (user == null)

                return Json(JsonResultResponse.ResponseFail("Verification failed. Login failed."));

            return Json(JsonResultResponse.ResponseSuccess(user));
        }
    }
}
