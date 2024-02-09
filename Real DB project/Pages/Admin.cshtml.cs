using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace Real_DB_project.Pages
{
    public class AdminModel : PageModel
    {

        public string image { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a name with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a name with at most 15 letters")]     
        public string EmpName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a username with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a username with at most 15 letters")]
        public string EmpUsername { get; set; }


        [BindProperty]
        [MinLength(8, ErrorMessage = "Please enter a password with at least 8 letters")]
        [Required(ErrorMessage = "Field is Required")]
        public string EmpPassword { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a username with at least 3 letters.")]
        [MaxLength(15, ErrorMessage = "Please enter a username with at most 15 letters.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string EmpEmail { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a name with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a name with at most 15 letters")]
        public string CliUsername { get; set; }



        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a username with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a username with at most 15 letters")]
        public string CliName { get; set; }



        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [RegularExpression(@"^[\d\s()-]+$", ErrorMessage = "Invalid phone number format.")]
        [MinLength(7, ErrorMessage = "Please enter at least 7 characters.")]
        [MaxLength(15, ErrorMessage = "Please enter at most 15 characters.")]
        public string CliPhone { get; set; }



        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(8, ErrorMessage = "Please enter a password with at least 8 letters")]
        public string CliPass { get; set; }



        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "Invalid date format. Use yyyy/MM/dd.")]
        public string CliCreation { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Invalid Social Security Number format. Use 9 digits.")]
        public string Vssn { get; set; }



        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a name with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a name with at most 15 letters")]
        public string Vname { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [RegularExpression(@"^[\d\s()-]+$", ErrorMessage = "Invalid phone number format.")]
        [MinLength(7, ErrorMessage = "Please enter at least 7 characters.")]
        [MaxLength(15, ErrorMessage = "Please enter at most 15 characters.")]
        public string Vphone { get; set; }

        
        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a name with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a name with at most 15 letters")]
        public string Petname { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter only numbers")]
        public string Petid { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a name with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a name with at most 15 letters")]
        public string Petype { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [MinLength(3, ErrorMessage = "Please enter a name with at least 3 letters")]
        [MaxLength(15, ErrorMessage = "Please enter a name with at most 15 letters")]
        public string Petspecies { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Field is Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter only numbers")]
        public string Petage { get; set; }
        [BindProperty]
        public string Petstatus { get; set; }

        public List<EmployeeInfo> Employees { get; set; }
        public int EmployCount { get; set; }

        public List<VetInfo> Vets { get; set; }
        public int VetCount { get; set; }

        public List<ClientInfo> Clients { get; set; }
        public int ClientCount { get; set; }

        public List<PetInfo> Pets { get; set; }
        public int PetCount { get; set; }

        public class PetInfo
        {
            public string PetName { get; set; }
            public string PetID { get; set; }
            public string PetType { get; set; }
            public string PetSpecies { get; set; }
            public string PetAge { get; set; }
            public string PetStatus { get; set; }

        }

        public class EmployeeInfo
        {
            [BindProperty]
            public string Name { get; set; }
            [BindProperty]
            public string Username { get; set; }
            [BindProperty]
            public string Password { get; set; }
            [BindProperty]
            public string Email { get; set; }
        }

        public class VetInfo
        {
            public string VetName { get; set; }
            public int VetSSN { get; set; }
            public int VetPhone { get; set; }

        }


        public class ClientInfo
        {
            public string ClientName { get; set; }
            public string ClientUsername { get; set; }
            public string ClientPassword { get; set; }
            public int ClientPhone { get; set; }
            public string CreationDate { get; set; }
        }


        private void LoadData() //made this function bec after input validation, when the page reloads, the tables all become empty
        //But if we call this function again after before returning page, tables are full again
        {
		string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";


			SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string query1 = "SELECT COUNT(*) FROM Employee";
                SqlCommand CountCmd = new SqlCommand(query1, connection);
                EmployCount = (int)CountCmd.ExecuteScalar();

                string query2 = "SELECT * FROM Employee";
                SqlCommand infoCmd = new SqlCommand(query2, connection);

                Employees = new List<EmployeeInfo>();

                SqlDataReader reader = infoCmd.ExecuteReader();
                while (reader.Read())
                {
                    Employees.Add(new EmployeeInfo
                    {
                        Name = reader["NAME"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
                reader.Close();

                string vet = "SELECT COUNT(*) FROM Vet";
                SqlCommand CountV = new SqlCommand(vet, connection);
                VetCount = (int)CountV.ExecuteScalar();

                string vet2 = "SELECT * FROM Vet";
                SqlCommand Cmdvet = new SqlCommand(vet2, connection);

                Vets = new List<VetInfo>();

                SqlDataReader readerVet = Cmdvet.ExecuteReader();
                while (readerVet.Read())
                {
                    Vets.Add(new VetInfo
                    {
                        VetName = readerVet["VName"].ToString(),
                        VetSSN = (int)readerVet["SSN"],
                        VetPhone = (int)readerVet["VPhoneNumber"],

                    });
                }
                readerVet.Close();


                string ClientQ = "SELECT COUNT(*) FROM Client";
                SqlCommand CountC = new SqlCommand(ClientQ, connection);
                ClientCount = (int)CountC.ExecuteScalar();

                string ClientQ2 = "SELECT * FROM Client";
                SqlCommand infoClient = new SqlCommand(ClientQ2, connection);

                Clients = new List<ClientInfo>();

                SqlDataReader ClientReader = infoClient.ExecuteReader();
                while (ClientReader.Read())
                {
                    Clients.Add(new ClientInfo
                    {
                        ClientName = ClientReader["CName"].ToString(),
                        ClientUsername = ClientReader["CUsername"].ToString(),
                        ClientPassword = ClientReader["CPassword"].ToString(),
                        ClientPhone = (int)ClientReader["CPhoneNumber"],
                        CreationDate = ClientReader["AccounCreationDate"].ToString()
                    });
                }
                ClientReader.Close();


                string pet = "SELECT COUNT(*) FROM Pet";
                SqlCommand CountP = new SqlCommand(pet, connection);
                PetCount = (int)CountP.ExecuteScalar();

                string pet2 = "SELECT * FROM Pet";
                SqlCommand Cmdpet = new SqlCommand(pet2, connection);

                Pets = new List<PetInfo>();

                SqlDataReader readerPet = Cmdpet.ExecuteReader();
                while (readerPet.Read())
                {
                    Pets.Add(new PetInfo
                    {
                        PetName = readerPet["Pname"].ToString(),
                        PetID = readerPet["PetID"].ToString(),
                        PetType = readerPet["PetType"].ToString(),
                        PetSpecies = readerPet["PetSpecies"].ToString(),
                        PetAge = readerPet["Age"].ToString(),
                        PetStatus = readerPet["AdoptionStatus"].ToString()
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

        public void OnGet()
        {
            LoadData();
        }




        public IActionResult OnPostAddEmployee()
        {
            if (!ModelState.IsValid)
            {
                ModelState.Remove(nameof(Vname));
                ModelState.Remove(nameof(Vssn));
                ModelState.Remove(nameof(Vphone));
                ModelState.Remove(nameof(CliCreation));
                ModelState.Remove(nameof(CliName));
                ModelState.Remove(nameof(CliPass));
                ModelState.Remove(nameof(CliPhone));
                ModelState.Remove(nameof(CliUsername));
                ModelState.Remove(nameof(Petage));
                ModelState.Remove(nameof(Petid));
                ModelState.Remove(nameof(Petspecies));
                ModelState.Remove(nameof(Petname));
                ModelState.Remove(nameof(Petstatus));
                ModelState.Remove(nameof(Petype));
                //LoadData();
                //return Page();
            }


			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";

			SqlConnection conn = new SqlConnection(connectionString);
            string queryRow1 = "Insert into Employee values(@Username,@NAME, @Email, @Password)";
            SqlCommand cmd = new SqlCommand(queryRow1, conn);

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20).Value = EmpUsername;
            cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 20).Value = EmpName;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 30).Value = EmpEmail;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 20).Value = EmpPassword;


            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                

            }
            finally
            {              
               conn.Close();

            }
            return RedirectToPage();
        }


        public IActionResult OnPostAddClient()
        {
            if (!ModelState.IsValid)
            {
                ModelState.Remove(nameof(Vname));
                ModelState.Remove(nameof(Vssn));
                ModelState.Remove(nameof(Vphone));
                ModelState.Remove(nameof(Petage));
                ModelState.Remove(nameof(Petid));
                ModelState.Remove(nameof(Petspecies));
                ModelState.Remove(nameof(Petype));
                ModelState.Remove(nameof(Petname));
                ModelState.Remove(nameof(Petstatus));
                ModelState.Remove(nameof(Petname));
                ModelState.Remove(nameof(EmpEmail));
                ModelState.Remove(nameof(EmpUsername));
                ModelState.Remove(nameof(EmpPassword));
                ModelState.Remove(nameof(EmpName));
                LoadData();
                return Page();
            }





			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";

			SqlConnection conn = new SqlConnection(connectionString);
            string queryClient = "Insert into Client values(@CUsername,@Cname, @CPhone, @CPassword, @AccountCreation)";
            SqlCommand ClientCmd = new SqlCommand(queryClient, conn);

            ClientCmd.Parameters.Add("@CUsername", SqlDbType.NVarChar, 20).Value = CliUsername;
            ClientCmd.Parameters.Add("@Cname", SqlDbType.NVarChar, 20).Value = CliName;
            ClientCmd.Parameters.Add("@CPhone", SqlDbType.NVarChar, 15).Value = CliPhone;
            ClientCmd.Parameters.Add("@CPassword", SqlDbType.NVarChar, 20).Value = CliPass;
            ClientCmd.Parameters.Add("@AccountCreation", SqlDbType.NVarChar, 30).Value = CliCreation;


            try
            {
                conn.Open();
                ClientCmd.ExecuteNonQuery();

            }
            finally
            {

                conn.Close();

            }
            return RedirectToPage();
        }

        public IActionResult OnPostAddVet()
        {
            if (!ModelState.IsValid)
            {
                ModelState.Remove(nameof(CliCreation));
                ModelState.Remove(nameof(CliName));
                ModelState.Remove(nameof(CliPass));
                ModelState.Remove(nameof(CliPhone));
                ModelState.Remove(nameof(CliUsername));
                ModelState.Remove(nameof(Petage));
                ModelState.Remove(nameof(Petid));
                ModelState.Remove(nameof(Petspecies));
                ModelState.Remove(nameof(Petname));
                ModelState.Remove(nameof(Petstatus));
                ModelState.Remove(nameof(Petype));
                ModelState.Remove(nameof(EmpEmail));
                ModelState.Remove(nameof(EmpUsername));
                ModelState.Remove(nameof(EmpPassword));
                ModelState.Remove(nameof(EmpName));
                LoadData();
                return Page();
            }



			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";
			SqlConnection conn = new SqlConnection(connectionString);
            string queryVet = "Insert into Vet values(@SSN,@Name, @VPhone)";
            SqlCommand VetCmd = new SqlCommand(queryVet, conn);

            VetCmd.Parameters.Add("@SSN", SqlDbType.NVarChar, 9).Value = Vssn;
            VetCmd.Parameters.Add("@Name", SqlDbType.NVarChar, 20).Value = Vname;
            VetCmd.Parameters.Add("@VPhone", SqlDbType.NVarChar, 15).Value = Vphone;

            try
            {
                conn.Open();
                VetCmd.ExecuteNonQuery();

            }
            finally
            {

                conn.Close();

            }
            return RedirectToPage();
        }


        public IActionResult OnPostAddPet(IFormFile fileToUpload)
        {
            //if (!ModelState.IsValid)
            //{
                ModelState.Remove(nameof(Vphone));
                ModelState.Remove(nameof(Vssn));
                ModelState.Remove(nameof(Vname));
                ModelState.Remove(nameof(CliCreation));
                ModelState.Remove(nameof(CliName));
                ModelState.Remove(nameof(CliPass));
                ModelState.Remove(nameof(CliPhone));
                ModelState.Remove(nameof(CliUsername));
                ModelState.Remove(nameof(EmpEmail));
                ModelState.Remove(nameof(EmpUsername));
                ModelState.Remove(nameof(EmpPassword));
                ModelState.Remove(nameof(EmpName));
        //        LoadData();
        //        return Page();
        //}


			string connectionString = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";


			SqlConnection conn = new SqlConnection(connectionString);
            string queryRow1 = "Insert into Pet (PetID,PName,Age,AdoptionStatus,PetType, PetSpecies) values(@id,@name, @age, 'Unadopted',@type,@species)";
            SqlCommand cmd = new SqlCommand(queryRow1, conn);

            cmd.Parameters.Add("@name", SqlDbType.NVarChar, 20).Value = Petname;
            cmd.Parameters.Add("@id", SqlDbType.NVarChar, 20).Value = Petid;
            cmd.Parameters.Add("@type", SqlDbType.NVarChar, 30).Value = Petype;
            cmd.Parameters.Add("@species", SqlDbType.NVarChar, 20).Value = Petspecies;
            cmd.Parameters.Add("@age", SqlDbType.NVarChar, 30).Value = Petage;



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
                conn.Open();
                cmd.ExecuteNonQuery();


            }
            finally
            {
                conn.Close();

            }
            return RedirectToPage();
        }





    }
}
