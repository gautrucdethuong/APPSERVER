namespace RoleBasedAuthorization.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        /*if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.username == user.username))
                {
                    return base.Content("Username " + user.username + " is already exist. Please enter a different username.");
                }
                else if (db.Users.Any(x => x.email == user.email))
                {
                    return base.Content("Email " + user.email + " is already exist. Please enter a different email.");
                }
                else if (db.Users.Any(x => x.phone == user.phone))
                {
                    return base.Content("Number phone " + user.phone + " is already exist. Please enter a different number phone.");
                }
            }*/
    }
}