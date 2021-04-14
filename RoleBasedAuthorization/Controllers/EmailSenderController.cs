using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSenderController : Controller
    {
        private IEmailSenderService emailSenderService;


        public EmailSenderController(IEmailSenderService emailSender)
        {
            emailSenderService = emailSender;
        }

        /*[HttpPost]
        public JsonResult SendPasswordResetLink(User user)
        {
            var users = forgotPasswordService.SendPasswordResetLink(user.email.ToString());
            MyIdentityUser user = userManager.
                 FindByNameAsync(obj.UserName).Result;

            if (user == null)
                return Json(JsonResultResponse.ResponseFail($"User with id {id} was not found."));

            return Json(JsonResultResponse.ResponseSuccess(user));
            

        }*/

    }
}
