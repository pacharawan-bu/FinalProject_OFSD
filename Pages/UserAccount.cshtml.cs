using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using static Final_new.Pages.IndexModel;
 
namespace Final_new.Pages
{
    public class UserAccountModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public List<UserInfo> listUser = new List<UserInfo>();
 
        private readonly ILogger<UserAccountModel> _logger;
 
        public UserAccountModel(ILogger<UserAccountModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            userInfo.username = Request.Form["username"];
            userInfo.pwd = Request.Form["pwd"];
            userInfo.fname = Request.Form["fname"];
            userInfo.lname = Request.Form["lname"];
            userInfo.jposition = Request.Form["jposition"];
            userInfo.numb = Request.Form["numb"];
            String confirmPwd = Request.Form["confirmPwd"];
 
            if (string.IsNullOrWhiteSpace(userInfo.username) || string.IsNullOrWhiteSpace(userInfo.pwd) ||
                string.IsNullOrWhiteSpace(userInfo.fname) || string.IsNullOrWhiteSpace(userInfo.lname) ||
                string.IsNullOrWhiteSpace(userInfo.jposition) || string.IsNullOrWhiteSpace(userInfo.numb))
            {
                errorMessage = "All fields are required.";
                return;
            }
 
            if (userInfo.pwd != confirmPwd)
            {
                errorMessage = "Passwords do not match.";
                return;
            }
 
            try
            {
                String connectionString = "Server = tcp:finalprojectofsd.database.windows.net,1433; Initial Catalog = FinalProjectOFSD; Persist Security Info = False; User ID = adminOFSD; Password = ofsd_1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkUsernameSql = "SELECT COUNT(*) FROM loginTable WHERE username = @username";
                    using (SqlCommand checkCommand = new SqlCommand(checkUsernameSql, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@username", userInfo.username);
 
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            errorMessage = "Username already exists.";
                            return;
                        }
                    }
                    String sql = "INSERT INTO loginTable (username, pwd, fname, lname, jposition, numb) " +
                                 "VALUES (@username, @pwd, @fname, @lname, @jposition, @numb)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", userInfo.username);
                        command.Parameters.AddWithValue("@pwd", userInfo.pwd);
                        command.Parameters.AddWithValue("@fname", userInfo.fname);
                        command.Parameters.AddWithValue("@lname", userInfo.lname);
                        command.Parameters.AddWithValue("@jposition", userInfo.jposition);
                        command.Parameters.AddWithValue("@numb", userInfo.numb);
 
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
 
            successMessage = "Account created successfully!";
            Response.Redirect("LoginPage");
        }
    }
}