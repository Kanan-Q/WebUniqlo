using Microsoft.AspNetCore.Identity;
using WebUniqlo.DataAccess;
using WebUniqlo.Enums;
using WebUniqlo.Models;

namespace WebUniqlo.Extension
{
    public static class SeedExtension
    {
        public static void UseUserSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               

                if (!RoleManager.Roles.Any())
                {
                    foreach (Roles item in Enum.GetValues(typeof(Roles)))
                    {
                        RoleManager.CreateAsync(new IdentityRole(item.ToString())).Wait();
                    }
                }
                if (!UserManager.Users.Any(x => x.NormalizedUserName == "ADMIN"))
                {
                    User u=new User
                    {
                        FullName = "admin",
                        UserName = "admin",
                        Email = "admin@mail.ru",
                        ImageURL = "photo.jpg"
                    };
                    UserManager.CreateAsync(u, "" + "+Aa123***").Wait();
                    UserManager.AddToRoleAsync(u,nameof(Roles.Admin)).Wait();
                }
            }
        }
    }
}
