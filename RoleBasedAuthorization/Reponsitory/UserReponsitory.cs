using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Helpers;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System.Collections.Generic;
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
                edituser.user_fullname = user.user_fullname;
                edituser.user_email = user.user_email;
                edituser.user_password = user.user_password;
                edituser.user_phone = user.user_phone;
                //edituser.user_avt = user.user_avt;
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
        public bool CheckExistProperties(string email, string username, string phone)
        {            
            if (db.Users.Any(x => x.user_username == username || x.user_email == email || x.user_phone == phone))
            {
                return false;
            }           
            return true;
        }       


        // search user by phone number or fullname
        public IEnumerable<User> searchUser(string name, string phone)
        {
            IQueryable<User> query = db.Users;

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(phone))
                return null;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                query = query.Where(x => x.user_fullname.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                phone = phone.Trim();
                query = query.Where(x => x.user_phone.Contains(phone));
            }
            return query.ToList();
        }

        // filter by role
        public IEnumerable<User> filterUserByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return null;
            
            return db.Users.Where(x => x.user_role == role.Trim()).ToList();
        }
    }
}
