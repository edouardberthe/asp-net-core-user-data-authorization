using System.Threading.Tasks;
using AspNetCoreUserDataAuthorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreUserDataAuthorization.Authorization
{
    public sealed class ContactIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Contact>
    {
        private readonly UserManager<IdentityUser> userManager;

        public ContactIsOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.CreateOperationName && 
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID == userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}