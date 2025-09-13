using DAL;
using DAL.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Ui.Services
{
    public class ContextConfig
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";

        public static async Task SeedDataAsync(ShippingContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedUserAsync(userManager, roleManager);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            //------------------------------------------------------

            if (!await roleManager.RoleExistsAsync("Reviewer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Reviewer"));
            }

            if (!await roleManager.RoleExistsAsync("Operation"))
            {
                await roleManager.CreateAsync(new IdentityRole("Operation"));
            }


            if (!await roleManager.RoleExistsAsync("Operation Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("Operation Manager"));
            }



            // Ensure admin user exists
            var adminEmail = seedAdminEmail;
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var id = Guid.NewGuid().ToString();
                adminUser = new ApplicationUser
                {
                    Id = id,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                };
                string password = "admin123456";
                var result = await userManager.CreateAsync(adminUser,password);
                await userManager.AddToRoleAsync(adminUser, "Admin");

            }



            var ReviewerEmail = "ReviewerUser@gmail.com";
            var ReviewerUser = await userManager.FindByEmailAsync(ReviewerEmail);
            if (ReviewerUser == null)
            {
                var id = Guid.NewGuid().ToString();
                ReviewerUser = new ApplicationUser
                {
                    Id = id,
                    FirstName = "Reviewer",
                    LastName = "User",                    
                    UserName = ReviewerEmail,
                    Email = ReviewerEmail,
                    EmailConfirmed = true,
                };
                string password = "admin123456";
                var result = await userManager.CreateAsync(ReviewerUser, password);
                await userManager.AddToRoleAsync(ReviewerUser, "Reviewer");

            }



            var OperationEmail = "OperationUser@gmail.com";
            var OperationUser = await userManager.FindByEmailAsync(OperationEmail);
            if (OperationUser == null)
            {
                var id = Guid.NewGuid().ToString();
                OperationUser = new ApplicationUser
                {
                    Id = id,
                    FirstName = "Operation",
                    LastName = "User",
                    UserName = OperationEmail,
                    Email = OperationEmail,
                    EmailConfirmed = true,
                };
                string password = "admin123456";
                var result = await userManager.CreateAsync(OperationUser, password);
                await userManager.AddToRoleAsync(OperationUser, "Operation");

            }




            var OperationMangEmail = "OperationMangUser@gmail.com";
            var OperationMangUser = await userManager.FindByEmailAsync(OperationMangEmail);
            if (OperationMangUser == null)
            {
                var id = Guid.NewGuid().ToString();
                OperationMangUser = new ApplicationUser
                {
                    Id = id,
                    FirstName = "OperationMang",
                    LastName = "User",
                    UserName = OperationMangEmail,
                    Email = OperationMangEmail,
                    EmailConfirmed = true,
                };
                string password = "admin123456";
                var result = await userManager.CreateAsync(OperationMangUser, password);
                await userManager.AddToRoleAsync(OperationMangUser, "Operation Manager");

            }





        }
    }
}
