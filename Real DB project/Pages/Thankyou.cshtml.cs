using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Real_DB_project.Pages
{
    public class ThankyouModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost() {

            return RedirectToPage("/Index");
        }
    }
}
