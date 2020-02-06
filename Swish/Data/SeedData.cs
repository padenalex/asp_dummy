using Swish.Authorization;
using Swish.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.WebEncoders.Testing;

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
                //await EnsureRole(serviceProvider, userID, Constants.ReadOperationName);
                
                
                SeedDB(context, adminID, managerID, userID);
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

        public static void SeedDB(ApplicationDbContext context, string adminID, string managerID, string UserID)
        {
            if (context.VerifUsers.Any())
            {
                return;   // DB has been seeded
            }

            context.VerifUsers.AddRange(new VerifUser
                {
                    Id = 1,
                    UserId = adminID,
                    FirstName = "admin",
                    LastName = "sdf",
                    FakeImgStr = "aaaaaaa",
                    Status = VerificationProfileStatus.Approved
                }, new VerifUser
                {
                    Id = 2, 
                    UserId = managerID,
                    FirstName = "mang",
                    LastName = "sead",
                    FakeImgStr = "rewweer",
                    Status = VerificationProfileStatus.Approved
                }, new VerifUser
                {
                    Id = 3,
                    UserId = UserID,
                    FirstName = "user",
                    LastName = "bob",
                    FakeImgStr = "sdddddddd",
                    Status = VerificationProfileStatus.Approved
                }
            );
            
            context.VerifManagers.AddRange(new VerifManager
                {
                    UserId = adminID,
                    Name = "adminCo"
                }, new VerifManager 
                {
                    UserId = managerID,
                    Name = "ManagerCo"
                }
            );
            
            context.ManagerClaims.AddRange(new ManagerClaim
                {
                    ManagerId = 1,
                    UserId = 2
                }, new ManagerClaim
                {
                    ManagerId = 1,
                    UserId = 3
                }, new ManagerClaim
                {
                    ManagerId = 2,
                    UserId = 3
                }
            );
            
            context.UserClaims.AddRange(new UserClaim
                {
                    UserId = 2,
                    ManagerId = 1
                }, new UserClaim
                {
                    UserId = 3,
                    ManagerId = 1
                }, new UserClaim
                {
                    UserId = 3,
                    ManagerId = 2
                }
            );

            context.SaveChanges();
        }
    }
}
