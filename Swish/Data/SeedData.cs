using Swish.Authorization;
using Swish.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Swish.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@test.com");
                await EnsureRole(serviceProvider, adminID, Constants.UserAdministratorsRole);
                
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@test.com");
                await EnsureRole(serviceProvider, managerID, Constants.UserManagersRole);
                
                var userID = await EnsureUser(serviceProvider, testUserPw, "user@test.com");
                //await EnsureRole(serviceProvider, managerID, Constants.UserManagersRole);
                
                
                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }
            
            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.VerificationProfiles.Any())
            {
                return;   // DB has been seeded
            }

            context.VerificationProfiles.AddRange(new VerificationProfile
                {
                    FirstName = "Debra",
                    LastName = "bob",
                    FakeImgStr = "fdfdf",
                    Status = VerificationProfileStatus.Approved,
                    MId = adminID
                }, new VerificationProfile
                {
                    FirstName = "Debra",
                    LastName = "saaab",
                    FakeImgStr = "fdfdsadasdasdf",
                    Status = VerificationProfileStatus.Approved,
                    MId = adminID
                }, new VerificationProfile
             {
                     FirstName = "ssssssra",
                     LastName = "sdddd",
                     FakeImgStr = "cxvvv",
                     Status = VerificationProfileStatus.Approved,
                     MId = adminID
                 }, new VerificationProfile
             {
                 FirstName = "car",
                 LastName = "sead",
                 FakeImgStr = "rewweer",
                 Status = VerificationProfileStatus.Approved,
                 MId = adminID
             }, new VerificationProfile
             {
                 FirstName = "joe",
                 LastName = "bob",
                 FakeImgStr = "sdddddddd",
                 Status = VerificationProfileStatus.Approved,
                 MId = adminID
             }
             );
            context.SaveChanges();
        }
    }
}
