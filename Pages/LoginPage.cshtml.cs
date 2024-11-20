using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Final_new.Pages
{
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                errorMessage = "Username and password are required.";
                return;
            }
            try
            {
                String connectionString = "Server = tcp:finalprojectofsd.database.windows.net,1433; Initial Catalog = FinalProjectOFSD; Persist Security Info = False; User ID = adminOFSD; Password = ofsd_1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM loginTable WHERE username = @username AND pwd = @password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", Username);
                        command.Parameters.AddWithValue("@password", Password);

                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            HttpContext.Session.SetString("username", Username);
                            successMessage = "Login successful!";
                            Response.Redirect("/ViewEmail"); 
                        }
                        else
                        {
                            
                            errorMessage = "Invalid username or password.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }


        }
    }
}