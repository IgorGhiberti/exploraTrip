using System.Net;
using System.Net.Mail;

namespace Application.Common;

public static class SendEmailHelper
{
    public static void SendEmail(string userEmail, string body, string subject)
    {
        try
        {
            using (SmtpClient smtp = new SmtpClient())
        {
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("igughiberti@gmail.com", "myeu wqtw glhg beol");

            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("igughiberti@gmail.com", "Explora Trip");
                msg.To.Add(new MailAddress(userEmail));
                msg.Subject = subject;
                msg.Body = body;
                smtp.Send(msg);
            }
        }
        }
        catch (System.Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
}

