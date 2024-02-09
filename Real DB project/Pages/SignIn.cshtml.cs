using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Real_DB_project.Models;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Numerics;
using System.Xml.Linq;

// :)

namespace Real_DB_project.Pages
{


    public class SignInModel : PageModel
    {
        private readonly DB db;
        public DataTable dt { get; set; }

        [BindProperty]
        [Required]
        [MinLength(4, ErrorMessage = "Length must be minimum 4 letters")]
        public string Username { get; set; }
        [BindProperty]
        [Required]
        [MinLength(4, ErrorMessage = "Length must be minimum 4 letters")]
        public string Password { get; set; }


        [BindProperty]
        [Required]
        [MinLength(4, ErrorMessage = "Length must be minimum 4 letters")]
        public string SUusername { get; set; }
        [BindProperty]
        [Required]
        [MinLength(4, ErrorMessage = "Length must be minimum 4 letters")]
        [MaxLength(12, ErrorMessage = "Length must be maximum 12 letters")]
        public string SUpassword { get; set; }
        [BindProperty]
        [Required]
        [MinLength(4, ErrorMessage = "Length must be minimum 4 letters")]
        [MaxLength(12, ErrorMessage = "Length must be maximum 12 letters")]
        public string SUname { get; set; }
        [BindProperty]
        [Required]
        [StringLength(11, ErrorMessage = "Length must be 11 numbers")]
        public string SUphone { get; set; } // changed to string
        [BindProperty(SupportsGet = true)]
        public string msg { get; set; }
        [BindProperty(SupportsGet = true)]
        public string msg2 { get; set; }



        //add a type to know if client w keda for donation w follow up
        public SignInModel(ILogger<IndexModel> logger, DB db)    //to connect to db
        {
            //_logger = logger;
            this.db = db;

        }

        public void OnGet()
        {
        }
        public IActionResult OnPostSigninbtn()
        {

            ModelState.Remove(nameof(msg));
            ModelState.Remove(nameof(msg2));
            ModelState.Remove(nameof(SUname));
            ModelState.Remove(nameof(SUphone));
            ModelState.Remove(nameof(SUpassword));
            ModelState.Remove(nameof(SUusername));

            if (ModelState.IsValid)
            {


                Console.WriteLine(Username + " " + Password);
                if (Username == "Admin" && Password == "admin")
                    return RedirectToPage("/Admin");
                string type = db.GetLoginInfo(Username, Password);
                if (type == "client")
                    return RedirectToPage("/IndexSignedIn", new { ClientUser = Username });     
                else if (type == "handler")
                    return RedirectToPage("/Employee", new { EmpUsername = Username });
                else
                    //show error
                    return RedirectToPage("/SignIn", new { msg = "The Username or Password are incorrect!" });
            }
            // for debuging
            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    Console.WriteLine($"Validation error for {key}: {error.ErrorMessage}");
                }
            }

            return Page();
        }

        public IActionResult OnPostSignup()

        {
            ModelState.Remove(nameof(msg));
            ModelState.Remove(nameof(msg2));
            ModelState.Remove(nameof(Username));
            ModelState.Remove(nameof(Password));


            if (ModelState.IsValid)
            {
                Console.WriteLine(SUname + " " + SUphone + " " + SUpassword + " " + SUusername);
                db.AddNewUser(SUusername, SUpassword, SUname, int.Parse(SUphone));
                //msg = "You have created an account! you can sign in now.";
                return RedirectToPage("/Index");

                var currentdate = DateTime.Now.ToString("yyyy/MM/dd");
				string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";

				SqlConnection conn = new SqlConnection(connectionString);
                string queryRow1 = "Insert into Client values(@CUsername,@CName, @CPassword,@CPhoneNumber,@AccountCreationDate)";
                SqlCommand cmd = new SqlCommand(queryRow1, conn);

                cmd.Parameters.Add("@CUsername", SqlDbType.NVarChar, 20).Value = SUusername;
                cmd.Parameters.Add("@CName", SqlDbType.NVarChar, 20).Value = SUname;
                cmd.Parameters.Add("@CPassword", SqlDbType.NVarChar, 30).Value = SUpassword;
                cmd.Parameters.Add("@CPhoneNumber", SqlDbType.NVarChar, 20).Value = SUphone;
                cmd.Parameters.Add("@AccountCreationDate", SqlDbType.NVarChar, 20).Value = currentdate;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();


                }
                finally
                {
                    conn.Close();

                }

            }
            return Page();
            //return RedirectToPage(new { msg2 = "All fields are required!" });


            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    Console.WriteLine($"Validation error for {key}: {error.ErrorMessage}");
                }
            }
            return RedirectToPage(new { msg2 = "All fields are required!" });


        }

    }
}