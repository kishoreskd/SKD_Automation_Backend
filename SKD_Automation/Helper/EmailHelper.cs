using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SKD_Automation.Helper
{
    public interface IEmailHelper
    {
        bool SendCredentialMail(string mailAddress, string userName, string password);
    }

    public class EmailHelper : IEmailHelper
    {
        public bool SendCredentialMail(string mailAddress, string userName, string password)
        {
            string msg = $"Hi\nSee below for your login credentials\n\n User name: {userName}\n Password: {password}";

            MailMessage mail = new MailMessage();

            string body_Message = $"Dear {userName.ToUpper()}," + Environment.NewLine + Environment.NewLine;
            body_Message += msg + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            body_Message += "Regards " + Environment.NewLine;
            body_Message += "PGT Automation." + Environment.NewLine + Environment.NewLine;


            mail.Subject = "PGT Dashbord Tracker Web Application Login Credentials";
            mail.Body = body_Message;
            mail.From = new MailAddress("pgtautomation@pangulftech.com");
            mail.To.Add(mailAddress);

            //SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");
            //SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.Port = 587;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("pgtautomation@pangulftech.com", "Aut0m@t3");
            //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            //SmtpServer.TargetName = "STARTTLS/smtp.office365.com";
            //SmtpServer.EnableSsl = true;

            //SmtpServer.SendAsync(mail, default);
            return true;
        }
    }
}
