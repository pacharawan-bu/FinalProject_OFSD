using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Final_new.Pages
{
    public class ComposeEmailModel : PageModel
    {
        public EmailInfo emailInfo = new EmailInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public List<EmailInfo> Emails = new List<EmailInfo>();


        private readonly ILogger<ComposeEmailModel> _logger;

        public ComposeEmailModel(ILogger<ComposeEmailModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            emailInfo.EmailSender = HttpContext.Session.GetString("username");
        }
        public void OnPost()
        {




            emailInfo.EmailSubject = Request.Form["subject"];
            emailInfo.EmailMessage = Request.Form["message"];
            emailInfo.EmailDate = DateTime.Now.ToString("yyyy-MM-dd");
            emailInfo.EmailIsRead = "N"; // Default: Unread
            emailInfo.EmailSender = HttpContext.Session.GetString("username");
            emailInfo.EmailReciever = Request.Form["emailreciever"];


            ////emailInfo.EmailID = Request.Form["emailid"];

            if (emailInfo.EmailSubject.Length == 0 || emailInfo.EmailReciever.Length == 0 ||
                emailInfo.EmailMessage.Length == 0 || emailInfo.EmailDate.Length == 0 || emailInfo.EmailIsRead.Length == 0
                || emailInfo.EmailSender.Length == 0)
            {
                errorMessage = "All the field are required";
                return;
            }

            if (emailInfo.EmailMessage.Length > 100)
            {
                errorMessage = "Email message cannot exceed 100 characters.";
                return;
            }

            try
            {
                String connectionString = "Server = tcp:finalprojectofsd.database.windows.net,1433; Initial Catalog = FinalProjectOFSD; Persist Security Info = False; User ID = adminOFSD; Password =ofsd_1234 ; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    String sql = "INSERT INTO Emails " +
                                "( EmailSubject, EmailMessage, EmailDate, EmailIsRead, EmailSender, EmailReciever) VALUES " +
                                "( @subject, @message, @date, @isread, @sender, @emailreciever );";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        
                        command.Parameters.AddWithValue("@subject", emailInfo.EmailSubject);
                        command.Parameters.AddWithValue("@message", emailInfo.EmailMessage);
                        command.Parameters.AddWithValue("@date", emailInfo.EmailDate);
                        command.Parameters.AddWithValue("@isread", emailInfo.EmailIsRead);
                        command.Parameters.AddWithValue("@sender", emailInfo.EmailSender);
                        command.Parameters.AddWithValue("@emailreciever", emailInfo.EmailReciever);

                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            //emailInfo.EmailID = "";
            emailInfo.EmailSubject = "";
            emailInfo.EmailMessage = "";
            emailInfo.EmailDate = "";
            emailInfo.EmailIsRead = "";
            emailInfo.EmailSender = "";
            emailInfo.EmailReciever = "";
            successMessage = "New Email was sent.";

            Response.Redirect("SentEmail");
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
        public String EmailReciever;
    }
}