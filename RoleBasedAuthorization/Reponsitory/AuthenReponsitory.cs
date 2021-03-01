using Microsoft.IdentityModel.Tokens;
using OAuth2;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Helpers;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace RoleBasedAuthorization.Reponsitory
{
    public class AuthenReponsitory : IAuthenticateService
    {
        private DBContext db;
        
        public AuthenReponsitory(DBContext dbContext)
        {
            db = dbContext;
            
        }
        
        // generation token
        public User Login(string username, string password)
        {

            List<User> users = db.Users.ToList();
            var user = users.SingleOrDefault(x => x.username == username && x.password == password);


            // return null if user not found
            if (user == null)
                return null;


            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Constant.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.user_id.ToString()),
                    new Claim(ClaimTypes.Role, user.role),
                    new Claim("Fullname", user.fullname),
                    new Claim("Username", user.username),
                    new Claim("Email", user.email),
                }),

                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            // write token
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }
     
    }
}
