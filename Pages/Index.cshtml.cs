using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Final_new.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            try
            {
                String connectionString = "Server=tcp:buem.database.windows.net,1433;Initial Catalog=buem;Persist Security Info=False;User ID=[USERNAME];Password=[PASSWORD];MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                { }
            }
            catch
            {
            }
        }
        public class UserInfo
        {
            public int id;
            public String username;
            public String pwd;
            public String fname;
            public String lname;
            public String jposition;
            public String numb;
        }
    }
}