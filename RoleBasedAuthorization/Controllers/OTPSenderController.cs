using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;


namespace RoleBasedAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPSenderController : Controller
    {
        private readonly IOTPSenderService _optsender;

        public OTPSenderController(IOTPSenderService senderService)
        {
            _optsender = senderService;
        }


        [HttpPost("sender")]
        public JsonResult SenderOTP([FromBody] OTPModel model)
        {           
            if (ModelState.IsValid)
            {
                if(_optsender.OTPSenderUser(model.phone) == true)
                {                    
                    return Json(JsonResultResponse.ResponseChange("OTP sent successed."));
                }                                
            }
            return Json(JsonResultResponse.ResponseFail("OTP sent failed."));            
        }


        [HttpPost("verify")]
        public JsonResult VerificationOTP([FromBody] VerifyModel model)
        {
            if (ModelState.IsValid)
            {
                if (_optsender.VerificationOTP(model.otp) == true)
                {
                    return Json(JsonResultResponse.ResponseChange("Verification successed."));
                }
            }
            return Json(JsonResultResponse.ResponseFail("Verification failed."));
        }
       
    }
}
