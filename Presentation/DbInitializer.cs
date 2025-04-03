using Common.Constants;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DbInitializer
    {
        public static void Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
           
            AddAdmin(userManager, roleManager);
        }
      
        private static void AddAdmin(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var roleResult = roleManager.CreateAsync(new IdentityRole("Admin")).Result;
                if (!roleResult.Succeeded)
                    throw new Exception("Admin rolunu yaratmaq olmadi");
            }


            if (userManager.FindByEmailAsync("admin@app.com").Result is null)
            {
                var user = new User
                {
                    UserName = "admin@app.com",
                    Email = "admin@app.com",
                    FullName="Admin"
                };
                user.EmailConfirmed= true;
                var result = userManager.CreateAsync(user, "Admin123!").Result;
                if (!result.Succeeded)
                    throw new Exception("Admin elave etmek mumkun olmadi");
                var role = roleManager.FindByNameAsync("Admin").Result;
                if (role?.Name is null)
                    throw new Exception("Admin rolu tapilmadi");

                var addToRoleResult = userManager.AddToRoleAsync(user, role.Name).Result;

                if (!addToRoleResult.Succeeded)
                    throw new Exception("Istifadeciye admin rolunu elave etmek mumkun olmadi");
                
            }

        }

        internal static void Seed(UserManager<User>? userManager, RoleManager<IdentityRole>? roleManager)
        {
            throw new NotImplementedException();
        }
    }
}
