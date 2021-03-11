using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RoleBasedAuthorization.Service;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Helper;


namespace RoleBasedAuthorization.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        
        private IUserService userService;
        

        public UserController(IUserService iuser)
        {
            userService = iuser;           
        }
       


        //get all list, allow only admin get alllist              
        [HttpGet]
        public JsonResult ListUser()
        {           
            var users = userService.getAllUser();
            return Json(JsonResultResponse.ResponseSuccess(users));
         
        }



        //get by id
        [HttpGet("{id}")]
        public JsonResult GetUser(int id)
        {
            var user = userService.GetUser(id);

            if (user == null)
                return Json(JsonResultResponse.ResponseFail($"User with id {id} was not found."));
            
            return Json(JsonResultResponse.ResponseSuccess(user));
            
        }


        //edit
        [HttpPut("{id}")]      
        public JsonResult EditUser(int id, User u)
        {
            var checkexist = userService.GetUser(id);

            if (checkexist != null)
            {
                u.user_id = checkexist.user_id;
                userService.EditUser(u);
            }
            return Json(JsonResultResponse.ResponseChange("Update Successed."));
        }


        //create
        [HttpPost, AllowAnonymous]       
        public JsonResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                if(userService.CheckExistProperties(user.email, user.username, user.phone) == "")
                {
                    userService.CreateUser(user);
                    return Json(JsonResultResponse.ResponseSuccess(user));
                }
            }
            return Json(JsonResultResponse.ResponseFail("Input data invalid."));

        }


        //delete
        [HttpDelete("{id}")]
        public JsonResult DeleteUser(int id)
        {
            var user = userService.GetUser(id);
            if (user == null)
            {               
                return Json(JsonResultResponse.ResponseFail($" User with id {id} was not found."));
            }
            userService.DeleteUser(user);
            return Json(JsonResultResponse.ResponseChange("Delete successed."));
        }      

    }
}
