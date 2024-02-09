using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Reflection.PortableExecutable;
using static Real_DB_project.Pages.AdminModel;

namespace Real_DB_project.Pages
{
	public class EmployeeModel : PageModel
	{

		[BindProperty(SupportsGet = true)]
		public string EmpUsername { get; set; }

		[BindProperty]
		public string RequestNum { get; set; }

		[BindProperty]
		public string SDate { get; set; }
		[BindProperty]
		public string SPetID { get; set; }
		[BindProperty]
		public string SFeedingTime { get; set; }
		[BindProperty]
		public string SVaccineTime { get; set; }
		[BindProperty]
		public string SMedicationTime { get; set; }
		public List<SchInfo> Schedules { get; set; }
		public List<RequestInfo> Requests { get; set; }

		public int SchCount { get; set; }
		public int ARCount { get; set; }
		[BindProperty]
		public string Status { get; set; }


		public class SchInfo
		{
			[BindProperty]
			public string Date { get; set; }
			[BindProperty]
			public int PetID { get; set; }
			[BindProperty]
			public string FeedingTime { get; set; }
			[BindProperty]
			public string VaccineTime { get; set; }
			[BindProperty]
			public string MedicationTime { get; set; }


		}

		
		public class RequestInfo
		{
			[BindProperty]
			public string RequestNum { get; set; }
			[BindProperty]
			public int PetID { get; set; }
			[BindProperty]
			public string RequestDate { get; set; }
			[BindProperty]
			public string ClientUsername { get; set; }



		}



		public void OnGet()
		{
			LoadData();
		}

		private void LoadData()
		{


			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";



			SqlConnection connection = new SqlConnection(connectionString);
			try
			{
				connection.Open();

				string query1 = "SELECT COUNT(*) FROM Schedule";
				SqlCommand CountCmd = new SqlCommand(query1, connection);
				SchCount = (int)CountCmd.ExecuteScalar();

				string query2 = "SELECT * FROM Schedule";
				SqlCommand infoCmd = new SqlCommand(query2, connection);

				Schedules = new List<SchInfo>();

				SqlDataReader reader = infoCmd.ExecuteReader();
				while (reader.Read())
				{
					Schedules.Add(new SchInfo
					{
						Date = reader["SDate"].ToString(),
						PetID = (int)reader["SPetID"],
						FeedingTime = reader["FeedingTime"].ToString(),
						VaccineTime = reader["VaccineTime"].ToString(),
						MedicationTime = reader["MedicationTime"].ToString()
					});
				}
				reader.Close();

				Requests ??= new List<RequestInfo>();

				string AR = "SELECT COUNT(*) FROM AdoptionRequest where Status ='Pending'";
				SqlCommand ARCmd = new SqlCommand(AR, connection);
				ARCount = (int)ARCmd.ExecuteScalar();

				string AR2 = "SELECT * FROM AdoptionRequest,Request where RequestNumber=ARequestNumber AND Status ='Pending'";
				SqlCommand InfoCmd2 = new SqlCommand(AR2, connection);

				Requests = new List<RequestInfo>();

				SqlDataReader reader1 = InfoCmd2.ExecuteReader();
				while (reader1.Read())
				{
					Requests.Add(new RequestInfo
					{
						RequestNum = reader1["RequestNumber"].ToString(),
						PetID = (int)reader1["APetID"],
						RequestDate = reader1["RequestDate"].ToString(),
						ClientUsername = reader1["ACUsername"].ToString(),

					});
				}
				reader1.Close();
			}

			finally
			{
				if (connection.State == ConnectionState.Open)
				{
					connection.Close();
				}
			}
		}


		public IActionResult OnPostAccept(string requestNum)
		{
			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";


			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string queryC = "UPDATE AdoptionRequest SET [Status] = 'Approved', EmpUserName = @empUsername WHERE RequestNumber = @requestNum";
				SqlCommand CCmd = new SqlCommand(queryC, conn);
				CCmd.Parameters.Add("@empUsername", SqlDbType.NVarChar, 20).Value = EmpUsername;
				CCmd.Parameters.Add("@requestNum", SqlDbType.NVarChar, 20).Value = requestNum;

				try
				{
					conn.Open();
					CCmd.ExecuteNonQuery();
				}
				finally
				{
					conn.Close();
				}
			}


			return RedirectToPage("/Index");
		}


		public IActionResult OnPostDecline(string requestNum)
		{
			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";



			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string queryC = "UPDATE AdoptionRequest SET [Status] = 'Declined', EmpUserName = @empUsername WHERE RequestNumber = @requestNum";
				SqlCommand CCmd = new SqlCommand(queryC, conn);
				CCmd.Parameters.Add("@empUsername", SqlDbType.NVarChar, 20).Value = EmpUsername;
				CCmd.Parameters.Add("@requestNum", SqlDbType.NVarChar, 20).Value = requestNum;

				try
				{
					conn.Open();
					CCmd.ExecuteNonQuery();
				}
				finally
				{
					conn.Close();
				}
			}

			return RedirectToPage("/Index");
		}
	}
}