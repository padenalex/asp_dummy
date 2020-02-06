using System.Security.Claims;
using Swish.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Swish.Authorization
{
    public class UserIsOwnerAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, VerifUser>
    {
        UserManager<IdentityUser> _userManager;

        public UserIsOwnerAuthorizationHandler(UserManager<IdentityUser> 
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                OperationAuthorizationRequirement requirement,
                VerifUser resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for CRUD permission, return.

            if (requirement.Name != Constants.ReadOperationName)
            {
                return Task.CompletedTask;
            }

           // if (resource. == _userManager.GetUserId(context.User))
           // {
           //     context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }
}