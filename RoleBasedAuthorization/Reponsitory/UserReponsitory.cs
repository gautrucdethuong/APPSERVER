using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Helpers;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace RoleBasedAuthorization.Reponsitory
{
    public class UserReponsitory : IUserService
    {      
        private DBContext db;
       

        public UserReponsitory(DBContext dbContext)
        {          
            db = dbContext;          
        }
        
        // create user           
        public User CreateUser(User user)
        {               
            db.Add(user);
            db.SaveChanges();
            return user;
        }


        //delete user
        public void DeleteUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }


        //edit user
        public User EditUser(User user)
        {
            var edituser = db.Users.Find(user.user_id);
            if (edituser != null)
            {
                db.Users.Update(edituser);
                edituser.username = user.username;
                edituser.fullname = user.fullname;
                edituser.email = user.email;
                edituser.password = user.password;
                edituser.phone = user.phone;
                db.SaveChanges();
            }
            return user;
        }
        //get all user
        public IEnumerable<User> getAllUser()
        {
            return db.Users.ToList();
        }
        //get user by id
        public User GetUser(int id)
        {
            var user = db.Users.Find(id);
            return user.WithoutPassword();
        }



        // check exist properties
        public object CheckExistProperties(string email, string username, string phone)
        {
            string message = "Input data invalid.";

            if (db.Users.Any(x => x.username == username))
            {
                return JsonResultResponse.ResponseFail(message);
                 
            }
            else if (db.Users.Any(x => x.email == email))
            {               
                return JsonResultResponse.ResponseFail(message); 
                //showMessage
            }
            else if (db.Users.Any(x => x.phone == phone))
            {              
                return JsonResultResponse.ResponseFail(message);
            }
            return ""; 
            
        }    
    }
}
