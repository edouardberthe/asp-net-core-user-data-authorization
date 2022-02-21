using AspNetCoreUserDataAuthorization.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreUserDataAuthorization.Pages.Contacts
{
    public abstract class BasePageModel : PageModel
    {
        protected ApplicationContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<IdentityUser> UserManager { get; }

        protected BasePageModel(ApplicationContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base()
        {
            Context = context;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }
    }
}