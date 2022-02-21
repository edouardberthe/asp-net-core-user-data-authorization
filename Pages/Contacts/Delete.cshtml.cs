using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreUserDataAuthorization.Models;
using AspNetCoreUserDataAuthorization.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreUserDataAuthorization.Authorization;

namespace AspNetCoreUserDataAuthorization.Pages.Contacts
{
    public class DeleteModel : BasePageModel
    {
        public DeleteModel(ApplicationContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Contact = await Context.Contacts.FirstOrDefaultAsync(m => m.ContactId == id);

            if (Contact == null)
            {
                return NotFound();
            }

            var isAuhtorized = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Delete);

            if (!isAuhtorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var contact = await Context
                .Contacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContactId == id);

            if (contact == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact, ContactOperations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Contacts.Remove(Contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
