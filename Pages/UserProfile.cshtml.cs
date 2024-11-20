using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using static Final_new.Pages.IndexModel;
using static Final_new.Pages.UserProfileModel;

namespace Final_new.Pages
{
    public class UserProfileModel : PageModel
    {

        public List<UserInfo> listUser = new List<UserInfo>();

        public void OnGet()
        {

            string loggedInUsername = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(loggedInUsername))
            {
                Console.WriteLine("User is not logged in.");
                return;
            }

            try
            {
                string connectionString = "Server = tcp:finalprojectofsd.database.windows.net,1433; Initial Catalog = FinalProjectOFSD; Persist Security Info = False; User ID = adminOFSD; Password = ofsd_1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string sql = "SELECT * FROM loginTable where Username = @username";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        command.Parameters.AddWithValue("@username", loggedInUsername);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    UserInfo userInfo = new UserInfo();
                                    userInfo.UserID = reader.GetInt32(0).ToString();
                                    userInfo.Username = reader.GetString(1);
                                    userInfo.pwd = reader.GetString(2);
                                    userInfo.fullname = reader.GetString(3)+"  "+reader.GetString(4);
                                    
                                    userInfo.position = reader.GetString(5);
                                    userInfo.number = reader.GetString(6);

                                    listUser.Add(userInfo);

                                    
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error reading email row: {ex.Message}");
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching emails: " + ex.Message);
            }


        }

        public class UserInfo
        {
            public String UserID;
            public String Username;
            public String pwd;
            public String fullname;
            
            public String position;
            public String number;
        }
    }
}