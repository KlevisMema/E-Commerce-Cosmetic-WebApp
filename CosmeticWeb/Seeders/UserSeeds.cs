using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CosmeticWeb.Seeders
{
    public static class UserSeeds
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            //Roles//
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("Employee"))
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            //Users//
            var getUsers = configuration.GetSection(UsersSettingsViewModel.SectionName).Get<UsersSettingsViewModel[]>();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            foreach (var item in getUsers)
            {
                var User = await userManager.FindByEmailAsync(item.UserName);

                if (User == null)
                {
                    var newUser = new IdentityUser()
                    {
                        UserName = item.UserName,
                        Email = item.UserName,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newUser, item.Password);

                    foreach (var role in item.Roles!)
                    {
                        await userManager.AddToRoleAsync(newUser, role);
                    }

                }
            }
        }
    }
}