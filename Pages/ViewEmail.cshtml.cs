using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using static Final_new.Pages.ViewEmailModel;

namespace Final_new.Pages
{
    public class ViewEmailModel : PageModel
    {
        public List<EmailInfo> listEmails = new List<EmailInfo>();

        public IActionResult OnPostReadEmail(string emailId)
        {
            TempData["EmailID"] = emailId; 
            return RedirectToPage("/ReadEmail"); 
        }

        public void OnGet()
        {

            String loggedInUsername = HttpContext.Session.GetString("username");
            if (String.IsNullOrEmpty(loggedInUsername))
            {
                Console.WriteLine("User is not logged in.");
                return;
            }

            try
            {
                String connectionString = "Server = tcp:finalprojectofsd.database.windows.net,1433; Initial Catalog = FinalProjectOFSD; Persist Security Info = False; User ID = adminOFSD; Password = ofsd_1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    String sql = @"
                    SELECT Emails.* 
                    FROM Emails 
                    INNER JOIN loginTable 
                    ON Emails.EmailReciever = loginTable.username 
                    WHERE loginTable.username = @username";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        command.Parameters.AddWithValue("@username", loggedInUsername);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    EmailInfo emailInfo = new EmailInfo();
                                    emailInfo.EmailID = reader.GetInt32(0).ToString();
                                    emailInfo.EmailSubject = reader.GetString(1);
                                    emailInfo.EmailMessage = reader.GetString(2);
                                    emailInfo.EmailDate = reader.GetString(3);
                                    emailInfo.EmailIsRead = reader.GetString(4);
                                    emailInfo.EmailSender = reader.GetString(5);
                                    emailInfo.EmailReceiver = reader.GetString(6);

                                    listEmails.Add(emailInfo);

                                    Console.WriteLine($"Email fetched: ID={emailInfo.EmailID}, Subject={emailInfo.EmailSubject}");
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
        public class EmailInfo
        {
            public String EmailID;
            public String EmailSubject;
            public String EmailMessage;
            public String EmailDate;
            public String EmailIsRead;
            public String EmailSender;
            public String EmailReceiver;
        }
    }
}