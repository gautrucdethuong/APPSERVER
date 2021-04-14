using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Service;

namespace RoleBasedAuthorization.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessageMMSController : Controller
    {
        private readonly IMessageService _message;

        public MessageMMSController(IMessageService message)
        {
            _message = message;
        }

       [HttpPost]
        public JsonResult SendMessageMMS()
        {
            _message.SendMMSMessage();
            return Json(JsonResultResponse.ResponseChange("Send message successed."));
        }
    }
}
