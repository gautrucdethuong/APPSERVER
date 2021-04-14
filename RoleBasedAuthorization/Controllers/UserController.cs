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
        
        private readonly IUserService _user;       

        public UserController(IUserService iuser)
        {
            _user = iuser;           
        }
       
        //get all list, allow only admin get alllist        
        [HttpGet]
        public JsonResult ListUser()
        {
            var users = _user.getAllUser();
            return Json(JsonResultResponse.ResponseSuccess(users));
         
        }


        //get by id
        [HttpGet("{id}")]
        public JsonResult GetUser(int id)
        {
            var user = _user.GetUser(id);

            if (user == null)
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            
            return Json(JsonResultResponse.ResponseSuccess(user));
            
        }

        //edit
        [HttpPut("{id}")]      
        public JsonResult EditUser(int id, User u)
        {
            var checkexist = _user.GetUser(id);

            if (checkexist != null)
            {
                u.user_id = checkexist.user_id;
                _user.EditUser(u);
            }
            return Json(JsonResultResponse.ResponseChange("Update Successed."));
        }

        //create
        [HttpPost, AllowAnonymous]       
        public JsonResult CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                if (_user.CheckExistProperties(user.user_email, user.user_username, user.user_phone) == true)
                {
                    _user.CreateUser(user);
                    return Json(JsonResultResponse.ResponseSuccess(user));
                }
            }
            return Json(JsonResultResponse.ResponseFail("Input data already exists."));
        }


        //delete
        [HttpDelete("{id}")]
        public JsonResult DeleteUser(int id)
        {
            var user = _user.GetUser(id);
            if (user == null)
            {               
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            _user.DeleteUser(user);
            return Json(JsonResultResponse.ResponseChange("Delete successed."));
        }
        

        // search by phone number or fullname
        [HttpGet("search"), AllowAnonymous]
        public JsonResult SearchUser(string name, string phone)
        {
            var search = _user.searchUser(name, phone);

            if (search == null)
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));

            return Json(JsonResultResponse.ResponseSuccess(search));
        }


        // filter by role
        [HttpGet("filter"), AllowAnonymous]
        public JsonResult filterUserByRole(string role)
        {
            var search = _user.filterUserByRole(role);

            if (search == null)
            {
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            return Json(JsonResultResponse.ResponseSuccess(search));
        }

    }
}
