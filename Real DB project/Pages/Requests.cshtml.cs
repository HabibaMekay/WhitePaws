using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Collections.Specialized.BitVector32;

namespace Real_DB_project.Pages
{
	public class RequestsModel : PageModel
	{
        public int ARCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PetID { get; set; }

        [BindProperty(SupportsGet = true)]

        public string ClientUser { get; set; }
        public List<PetInfo> Pets { get; set; }
        public class PetInfo
        {
            public string PetName { get; set; }
            public string PetID { get; set; }
            public string PetType { get; set; }
            public string PetSpecies { get; set; }
            public string PetAge { get; set; }

        }

        public void OnGet()
		{
			PetID = Request.Query["PetID"];
			ClientUser = Request.Query["ClientUser"];
			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";


			SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string pet2 = "SELECT * FROM Pet where PetID=@PetID";
                SqlCommand Cmdpet = new SqlCommand(pet2, connection);
                Cmdpet.Parameters.Add("@PetID", SqlDbType.NVarChar, 20).Value = PetID;
                SqlDataReader readerPet = Cmdpet.ExecuteReader();

                Pets = new List<PetInfo>();

                while (readerPet.Read())
                {
                    Pets.Add(new PetInfo
                    {
                        PetName = readerPet["Pname"].ToString(),
                        PetID = readerPet["PetID"].ToString(),
                        PetType = readerPet["PetType"].ToString(),
                        PetSpecies = readerPet["PetSpecies"].ToString(),
                        PetAge = readerPet["Age"].ToString(),
                    });
                }
                readerPet.Close();
            }

            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }



        public IActionResult OnPostSendRequest()
        {
            var CurrentDate = DateTime.Now.ToString("yyyy/MM/dd");

			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";

			using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string countQuery = "Select count(*) from AdoptionRequest";
                SqlCommand CountCmd = new SqlCommand(countQuery, conn);
                ARCount = (int)CountCmd.ExecuteScalar();

                string insertQuery = "insert into AdoptionRequest(RequestDate, Status, RequestNumber) values(@date, 'Pending', @requestnum)";
                SqlCommand Cmd = new SqlCommand(insertQuery, conn);
                Cmd.Parameters.Add("@requestnum", SqlDbType.NVarChar, 20).Value = ARCount + 1;
                Cmd.Parameters.Add("@date", SqlDbType.Date).Value = CurrentDate;


                string insertQuery2 = "insert into Request(ARequestNumber, APetID, ACUsername) values(@requestnum, @PetID, @Username)";
                SqlCommand Cmd2 = new SqlCommand(insertQuery2, conn);
                Cmd2.Parameters.Add("@requestnum", SqlDbType.NVarChar, 20).Value = ARCount + 1;
                Cmd2.Parameters.Add("@PetID", SqlDbType.NVarChar, 20).Value = PetID; 
                Cmd2.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = ClientUser; 

                try
                {
                    Cmd.ExecuteNonQuery();
                    Cmd2.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }

                return RedirectToPage("/Thankyou");
            }
        }







        public IActionResult OnPostContinue() {
            return RedirectToPage("/PetsSignedIn");
        }

	}
}


