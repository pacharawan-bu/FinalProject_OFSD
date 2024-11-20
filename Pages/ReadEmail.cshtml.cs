using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Final_new.Pages
{
    public class ReadEmailModel : PageModel
    {
        public EmailInfo EmailDetails { get; set; }

        public void OnGet(String emailId)
        {
            if (!String.IsNullOrEmpty(emailId))
            {
                FetchEmailDetails(emailId);
            }
        }
        private void FetchEmailDetails(String emailId)
        {
            try
            {
                
                try
                {
                    String connectionString = "Server=tcp:finalprojectofsd.database.windows.net,1433;Initial Catalog=FinalProjectOFSD;Persist Security Info=False;User ID=adminOFSD;Password=ofsd_1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = @"
                                SELECT EmailID, EmailSubject, EmailSender, EmailDate, EmailMessage 
                                FROM Emails 
                                WHERE EmailID = @EmailID
                                ORDER BY EmailSubject, EmailSender, EmailDate, EmailMessage";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@EmailID", emailId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    EmailDetails = new EmailInfo
                                    {
                                        EmailID = reader["EmailID"].ToString(),
                                        EmailSubject = reader["EmailSubject"].ToString(),
                                        EmailSender = reader["EmailSender"].ToString(),
                                        EmailDate = reader["EmailDate"].ToString(),
                                        EmailMessage = reader["EmailMessage"].ToString()
                                    };
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }

        }
    }

}
        public class EmailInfo
        {
            public String EmailID { get; set; }
            public String EmailSubject { get; set; }
            public String EmailSender { get; set; }
            public String EmailDate { get; set; }
            public String EmailMessage { get; set; }
        }
    
