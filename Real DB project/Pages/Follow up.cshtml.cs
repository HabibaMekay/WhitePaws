using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Real_DB_project.Pages.AdminModel;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Reflection;

namespace Real_DB_project.Pages
{
    public class Follow_upModel : PageModel
    {
		[BindProperty]
		public string health { get; set; }
		[BindProperty]
		public string image { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ClientUser { get; set; }
        public string ClientUser2  { get; set; }

        [BindProperty]
        public string PetID { get; set; }
        public int PetCount { get; set; }
        public List<PetInfo> PetsAdopted { get; set; }



        public void OnGet()
        {
			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";


			SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                
                string query1 = "Select count(*) from Request, AdoptionRequest where ARequestNumber=RequestNumber AND ACUsername=@ClientUser";
                SqlCommand CountCmd = new SqlCommand(query1, connection);
                CountCmd.Parameters.Add("@ClientUser", SqlDbType.NVarChar, 20).Value = ClientUser;
                PetCount = (int)CountCmd.ExecuteScalar();

                string query2 = "Select * from Request, AdoptionRequest where ARequestNumber=RequestNumber AND ACUsername=@ClientUser";
                SqlCommand infoCmd = new SqlCommand(query2, connection);
                infoCmd.Parameters.Add("@ClientUser", SqlDbType.NVarChar, 20).Value = ClientUser;
                PetsAdopted = new List<PetInfo>();

                SqlDataReader reader = infoCmd.ExecuteReader();
                while (reader.Read())
                {
                    PetsAdopted.Add(new PetInfo
                    {
                        PetID = reader["APetId"].ToString(),
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




        public IActionResult OnPostSubmit(IFormFile fileToUpload)
        {
            string clientUser = ClientUser;
            var CurrentDate = DateTime.Now.ToString("yyyy/MM/dd");

			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";


			SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string GetRequestNum = "select ARequestNumber from Request where ACUsername=@ClientUser AND APetID=@PetID";
                SqlCommand Cmd = new SqlCommand(GetRequestNum, conn);
                Cmd.Parameters.Add("@ClientUser", SqlDbType.NVarChar, 20).Value = clientUser;
                Cmd.Parameters.AddWithValue("@PetID", PetID);
                object result = Cmd.ExecuteScalar();

                string queryRow1 = "Insert into FollowUpReport(FDate,MedicalState, RequestNum) values(@date,@state, @requestnum)";
                SqlCommand cmd = new SqlCommand(queryRow1, conn);

                cmd.Parameters.Add("@date", SqlDbType.Date).Value = CurrentDate;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar, 20).Value = health;
                cmd.Parameters.Add("@requestnum", SqlDbType.NVarChar, 30).Value = result;

            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                // Specify the folder where you want to save the files
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Ensure the folder exists, create it if necessary
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Save the file to the server
                var filePath = Path.Combine(uploadFolder, fileToUpload.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileToUpload.CopyTo(stream);
                }

                // Optionally, you can do something with the file path here
                // For example, you might want to store it in ViewData for display on the page
                ViewData["FilePath"] = filePath;
            }

            try
            {
                
                cmd.ExecuteNonQuery();


            }
            finally
            {
                conn.Close();

            }
            return RedirectToPage("/Thankyou");
        }
    }



}
