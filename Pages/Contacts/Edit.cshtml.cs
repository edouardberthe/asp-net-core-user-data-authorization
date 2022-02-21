using System.Linq;
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
    public class EditModel : BasePageModel
    {
        public EditModel(ApplicationContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await Context.Contacts.FirstOrDefaultAsync(m => m.ContactId == id);

            if (Contact == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var contact = await Context
                .Contacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContactId == id);
            
            if (contact == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact, ContactOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Contact.OwnerID = contact.OwnerID;

            Context.Attach(Contact).State = EntityState.Modified;

            if (Contact.Status == ContactStatus.Approved)
            {
                var canApprove = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    Contact.Status = ContactStatus.Submitted;
                }
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(Contact.ContactId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ContactExists(int id)
        {
            return Context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
