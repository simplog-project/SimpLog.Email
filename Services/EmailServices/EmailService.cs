using SimpLog.Email.Models.AppSettings;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SimpLog.Email.Services.EmailServices
{
    internal static class EmailService
    {
        /// <summary>
        /// Send email on error messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendMail(string? message)
        {
            try
            {
                Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();
                var emailConfig = conf.Email_Configuration;
                var connection = emailConfig.Email_Connection;

                //  Check if at all is not disabled sending emails
                if (!IsEmailConfigurationValid(emailConfig))
                    return;

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailConfig.Email_From),
                    Subject = $"SimpLog Error - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                    Body = message ?? string.Empty,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailConfig.Email_To);

                if (!string.IsNullOrWhiteSpace(emailConfig.Email_Bcc))
                    mailMessage.Bcc.Add(emailConfig.Email_Bcc);

                if (!int.TryParse(connection.Port, out var port))
                    return; // or log error

                using var smtpClient = new SmtpClient(connection.Host)
                {
                    Port = int.Parse(connection.Port),
                    Credentials = new NetworkCredential(
                        connection.API_Key,
                        connection.API_Value),
                    EnableSsl = emailConfig.Enable_SSL ?? false
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                // log SMTP failure
            }
            catch (Exception ex)
            {
                // log unexpected failure
            }
        }

        private static bool IsEmailConfigurationValid(EmailConfiguration config)
        {
            return !string.IsNullOrWhiteSpace(config.Email_From)
                && !string.IsNullOrWhiteSpace(config.Email_To)
                && !string.IsNullOrWhiteSpace(config.Email_Connection.API_Key)
                && !string.IsNullOrWhiteSpace(config.Email_Connection.API_Value)
                && !string.IsNullOrWhiteSpace(config.Email_Connection.Host)
                && int.TryParse(config.Email_Connection.Port, out _);
        }
    }
}
