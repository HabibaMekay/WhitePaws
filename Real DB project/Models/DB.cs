using System.Data.SqlClient;
using System.Data;
namespace Real_DB_project.Models
{
    public class DB
    {
        public SqlConnection con { get; set; }
        public DB() 
        {
			string conStr = "Data Source=LAPTOP-8M8OHL36;Initial Catalog=PetProject;Integrated Security=True";

			con = new SqlConnection(conStr);
        }
        
        public DataTable GetTable(string tablename) //read specific table
        {
            DataTable dt = new DataTable();

            string q = "Select * From " + tablename;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(q, con);
                dt.Load(cmd.ExecuteReader());
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                con.Close();    
            }
            return dt;
        }

        public DataTable GetPetExplore() //for pet searching
        {
            DataTable dt = new DataTable();
            string q = "SELECT * FROM PET";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(q, con);
                dt.Load(cmd.ExecuteReader());
            }
            catch (SqlException ex)
            {
                Console.WriteLine( "Did not read from db :(" );
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
		public string GetLoginInfo(string username, string password) //for login only, returns type of user (client, admin, handler), and "none" if wrong credentials
		{
			DataTable dt = new DataTable();
			string q = "SELECT CUsername, CPassword FROM Client WHERE CUsername = '" +username+ "' AND CPassword = " +"'"+password + "'";
			try
			{
				con.Open();
				SqlCommand cmd = new SqlCommand(q, con);
				dt.Load(cmd.ExecuteReader());
			}
			catch (SqlException ex)
			{
				
				Console.WriteLine("Did not read from db :(");
			}
			finally
			{
				con.Close();
			}

            if(dt.Rows.Count == 1)  //client user found
            {
                return "client";
            }
			///////////////////////////////////////////////////////////////
			DataTable dt2 = new DataTable();
			q = "SELECT Username, Password FROM Employee WHERE Username = '" +username+ "' AND Password = " +"'"+password + "'";
			try
			{
				con.Open();
				SqlCommand cmd = new SqlCommand(q, con);
				dt2.Load(cmd.ExecuteReader());
			}

			catch (SqlException ex)
			{


				Console.WriteLine("Did not read from db :(");
			}

			finally
			{
				con.Close();
			}

			if (dt2.Rows.Count == 1)
			{
				return "handler";
			}

            // talk with group about the admin issue: how to know if handler or admin

			return "none";
		}
		public void AddNewUser(string username, string password, string name, int phone) //for pet searching
		{
			var currentdate = DateTime.Now.ToString("yyyy/MM/dd");
			DataTable dt = new DataTable();
			Console.WriteLine(currentdate.ToString());
			string q = "insert into Client (CName, CUsername, CPassword,CPhoneNumber,AccounCreationDate) values ('" + name+ "','" + username+ "','"+ password +"',"+phone+",'"+ currentdate + "');";
			try
			{
				con.Open();
				SqlCommand cmd = new SqlCommand(q, con);
				dt.Load(cmd.ExecuteReader());
			}
			catch (SqlException ex)
			{
				Console.WriteLine("Did not read from db :(");
			}
			finally
			{
				con.Close();
			}
		}
	}
}

 