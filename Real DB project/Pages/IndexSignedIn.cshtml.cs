using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Real_DB_project.Models;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace Real_DB_project.Pages
{
	public class IndexSingedInModel : PageModel

	{
		[BindProperty]
		[Required]
		[StringLength(16, ErrorMessage = "Credit card number should be 16 numbers")]
		public string creditcard { get; set; }
		[BindProperty]

		[StringLength(3, ErrorMessage = "CVV should be 3 numbers"), Required(ErrorMessage = "Field required")]
		public string cvv { get; set; }
		[BindProperty]

		[Required(ErrorMessage = "Field required")]
		public string amount { get; set; }

		[BindProperty(SupportsGet = true)]
		public string ClientUser { get; set; }






		private readonly ILogger<IndexModel> _logger;
		//private readonly DB db;
		//public DataTable dt { get; set; }



		public IndexSingedInModel(ILogger<IndexModel> logger) //, DB db)    //to connect to db
		{
			_logger = logger;
			//this.db = db;
		}

		public void OnGet()
		{

		}

		public IActionResult OnPostDonate()
		{
			if (ModelState.IsValid)
			{
				Console.WriteLine(ClientUser);
				var currentdate = DateTime.Now.ToString("yyyy/MM/dd");
				string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";

				SqlConnection conn = new SqlConnection(connectionString);
				string queryRow1 = "Insert into Donation values(@DDate, @ClientUsername, @Amount)";
				SqlCommand cmd = new SqlCommand(queryRow1, conn);

				cmd.Parameters.Add("@DDate", SqlDbType.NVarChar, 20).Value = currentdate;
				cmd.Parameters.Add("@ClientUsername", SqlDbType.NVarChar, 10).Value = ClientUser;
				cmd.Parameters.Add("@Amount", SqlDbType.NVarChar, 30).Value = amount;


				Console.WriteLine($"Current Date: {currentdate}");
				Console.WriteLine($"Client Username: {ClientUser}");
				Console.WriteLine($"Amount: {amount}");

				
				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();


				}
				finally
				{
					conn.Close();

				}
				//ToInt32(creditcard w cvv w amount ba2a w keda)
				return RedirectToPage("/Thankyou", new { ClientUser = ClientUser });        //password is not added in rawan's version
			}

			return Page();      //this might cause loss of username and password data
		}
	}


}