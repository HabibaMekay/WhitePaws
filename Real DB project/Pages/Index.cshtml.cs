using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Real_DB_project.Models;
using System.Data;


namespace Real_DB_project.Pages
{
    public class IndexModel : PageModel
    {
		public bool signin { get; set; }     //the one we use as a check
		[BindProperty(SupportsGet = true)]
		public bool signinreal { get; set; }        //gets value from signin page

        public string msg { get; set; }     //to say you successfully signed up, signed in, etc

        [BindProperty]
        public int creditcard { get; set; }
		[BindProperty]
		public int cvv { get; set; }
		[BindProperty]
		public int amount { get; set; }


        private readonly ILogger<IndexModel> _logger;
        //private readonly DB db;
        //public DataTable dt { get; set; }

        public IndexModel(ILogger<IndexModel> logger) //, DB db)    //to connect to db
        {
            _logger = logger;
            //this.db = db;
        }

        public void OnGet()
        {
			//msg = "";
			//if (signinreal == true)
			//	signin = true;
			//else signin = false;

   //         Console.WriteLine(signin.ToString());
		}

   //     public IActionResult OnPostDonate()
   //     {
			//Console.WriteLine(signin.ToString());

			//if (signin == true)
   //             return RedirectToPage("/Thankyou");
   //         else
   //             return RedirectToPage();
   //     }
    }
}