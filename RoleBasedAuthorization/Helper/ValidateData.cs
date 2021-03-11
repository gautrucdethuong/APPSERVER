using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Reponsitory;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace RoleBasedAuthorization.Helper
{
    public class IsValidData : ValidationAttribute
    {

        private DBContext db;

        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // UserReponsitory userReponsitory = new UserReponsitory(db);
            //            userReponsitory.
            

            string errorMessage = "Input Data Invalid.";

            var user = (User)validationContext.ObjectInstance;

            if (db.Users.Where(x => x.username == user.username).Any())
            {
                return new ValidationResult(errorMessage);

            }else if(db.Users.Where(x => x.email == user.email).Any())
            {
                return new ValidationResult(errorMessage);

            }else if (db.Users.Where(x => x.phone == user.phone).Any())
            {
                return new ValidationResult(errorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }                                                                       
        }
    }
}
