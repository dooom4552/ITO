using ITO.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ITO
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "Admin@";
            string password = "Trader25@";
            string governmentEmail = "Government@";
            string governmentPassword = "Trader24@";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await roleManager.FindByNameAsync("учреждение") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("учреждение"));
            } 
            if (await roleManager.FindByNameAsync("управление") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("управление"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {                  
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            
            if (await userManager.FindByNameAsync(governmentEmail) == null)
            {
                User government = new User { Email = governmentEmail, UserName = governmentEmail };
                IdentityResult result = await userManager.CreateAsync(government, governmentPassword);
                if (result.Succeeded)
                {                  
                    await userManager.AddToRoleAsync(government, "управление");
                }
            }
        }
    }
}