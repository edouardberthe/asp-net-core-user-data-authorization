using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreUserDataAuthorization.Models;
using AspNetCoreUserDataAuthorization.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreUserDataAuthorization.Authorization;

namespace AspNetCoreUserDataAuthorization.Pages.Contacts
{
    public class CreateModel : BasePageModel
    {
        public CreateModel(ApplicationContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Contact.OwnerID = UserManager.GetUserId(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Create);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Contacts.Add(Contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
