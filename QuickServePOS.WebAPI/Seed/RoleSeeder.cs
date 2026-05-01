using Microsoft.AspNetCore.Identity;

namespace QuickServePOS.WebAPI.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Admin",
                "Owner",
                "Waiter",
                "Cashier",
                "KitchenStaff",
                "Customer"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
