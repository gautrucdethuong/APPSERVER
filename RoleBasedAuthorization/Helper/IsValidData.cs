using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace RoleBasedAuthorization.Helper
{
    public class IsValidData : ValidationAttribute
    {
        
        /*protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DBContext db;
            if (value != null)
            {
                var valueAsString = value.ToString();
                IEnumerable<string> email = db.Users.Where(x => x.email != null).Select(x => x.email);
                if (email.Contains(valueAsString))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;

        }*/
    }
}
