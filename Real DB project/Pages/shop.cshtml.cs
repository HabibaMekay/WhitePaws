using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Real_DB_project.Models;
using System.Data;
namespace Real_DB_project.Pages
{

	public class shopModel : PageModel
	{
		public List<PetInfo> Pet { get; set; }
		public int PetCount { get; set; }


		private readonly ILogger<IndexModel> _logger;
		private readonly DB db;
		public DataTable dt { get; set; }

		public class PetInfo
		{
			[BindProperty]
			public string PetName { get; set; }
			[BindProperty]
			public string PetType { get; set; }

			[BindProperty]
			public string PetID { get; set; }


		}

		public shopModel(ILogger<IndexModel> logger, DB db)    //to connect to db
		{
			_logger = logger;
			this.db = db;
		}

		public void OnGet()
		{

			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";



			SqlConnection connection = new SqlConnection(connectionString);
			try
			{
				connection.Open();

				string query1 = "SELECT COUNT(*) FROM Pet";
				SqlCommand CountCmd = new SqlCommand(query1, connection);
				PetCount = (int)CountCmd.ExecuteScalar();

				string query2 = "SELECT * FROM Pet";
				SqlCommand infoCmd = new SqlCommand(query2, connection);

				Pet = new List<PetInfo>();

				SqlDataReader reader = infoCmd.ExecuteReader();
				while (reader.Read())
				{
					Pet.Add(new PetInfo
					{
						PetName = reader["PName"].ToString(),
						PetType = reader["PetType"].ToString(),
						PetID = reader["PetID"].ToString()
						//Password = reader["Password"].ToString(),
						//Email = reader["Email"].ToString()
					});
				}
				reader.Close();
			}
			finally
			{
				if (connection.State == ConnectionState.Open)
				{
					connection.Close();
				}
			}
		}
	}
}
