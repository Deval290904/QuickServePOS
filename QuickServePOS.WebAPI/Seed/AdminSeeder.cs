using Microsoft.AspNetCore.Identity;
using QuickServePOS.Models.Entities.Auth;

namespace QuickServePOS.WebAPI.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@gmail.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    Name = "System Admin",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}