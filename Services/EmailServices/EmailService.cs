using SimpLog.Email.Models.AppSettings;
using System;
using System.Net;
using System.Net.Mail;

namespace SimpLog.Email.Services.EmailServices
{
    internal static class EmailService
    {
        /// <summary>
        /// Send email on error messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void SendMail(string? message)
        {
            try
            {
                Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();

                //  Check if at all is not disabled sending emails
                if (string.IsNullOrEmpty(conf.Email_Configuration.Email_From) ||
                    string.IsNullOrEmpty(conf.Email_Configuration.Email_To) ||
                    string.IsNullOrEmpty(conf.Email_Configuration.Email_Connection.API_Key) ||
                    string.IsNullOrEmpty(conf.Email_Configuration.Email_Connection.API_Value) ||
                    string.IsNullOrEmpty(conf.Email_Configuration.Email_Connection.Host) ||
                    string.IsNullOrEmpty(conf.Email_Configuration.Email_Connection.Port))
                    return;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(conf.Email_Configuration.Email_From),
                    Subject = "SimpLog_" + DateTime.Now.DayOfYear.ToString(),
                    Body = message,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(new MailAddress(conf.Email_Configuration.Email_To));

                if (conf.Email_Configuration.Email_Bcc is not null)
                    mailMessage.Bcc.Add(new MailAddress(conf.Email_Configuration.Email_Bcc));

                var smtpClient = new SmtpClient(conf.Email_Configuration.Email_Connection.Host)
                {
                    Port = int.Parse(conf.Email_Configuration.Email_Connection.Port),
                    Credentials = new NetworkCredential(
                        userName: conf.Email_Configuration.Email_Connection.API_Key, 
                        password: conf.Email_Configuration.Email_Connection.API_Value),
                    EnableSsl = true,
                };

                smtpClient.Send(mailMessage);

                smtpClient.Dispose();
            }
            catch (SmtpFailedRecipientException ex)
            {
                //await ImediateSaveMessageIntoLogFile("Email was not send successfully and message is" + message, LogType.Error);
            }
        }
    }
}
