using DogsApp.Ifrastructure.Data.Domain;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsApp.Ifrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Client" };

            IdentityResult roleResult;

            foreach(var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);

                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedAdministator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if(await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "admin";
                user.LastName = "admin";
                user.PhoneNumber = "0888888888";
                user.UserName = "admin";
                user.Email = "admin@admin.com";

                var result = await userManager.CreateAsync(user, "Admin123456");

                if(result.Succeeded)
                {
                     userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;
            await RoleSeeder(services);
            await SeedAdministator(services);

            var data = serviceScope.ServiceProvider.GetRequiredService <ApplicationDbContext>();

            SeedBreeds(data);
            return app;
        }

        private static void SeedBreeds(ApplicationDbContext data)
        {
            if (data.Breeds.Any())
            {
                return;
            }
            data.Breeds.AddRange(new[]
            {
                new Breed {Name = "Husky"},
                new Breed {Name = "Pincher"},
                new Breed {Name = "Doberman"},
                new Breed {Name = "Good Boy"},
                new Breed {Name = "SUS"},
                new Breed {Name = "FromTheHOOD"}
            });
            data.SaveChanges();
        }
    }
}
