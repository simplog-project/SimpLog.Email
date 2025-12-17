using SimpLog.Email.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static SimpLog.Email.Models.Constants;

namespace SimpLog.Email.Services.FileServices
{
    internal class FileService
    {
        /// <summary>
        /// FullPath + FileName is the key and value is what should be saved into the log
        /// </summary>
        public static Dictionary<string, StringBuilder> Logs = new Dictionary<string, StringBuilder>();

        public static Models.AppSettings.Configuration configuration = ConfigurationServices.ConfigService.BindConfigObject();
        
        private static bool GetLogTypeEnabled(bool? value) => value ?? true;

        internal readonly bool? _Trace_Email  = GetLogTypeEnabled(configuration.LogType.Trace.SendEmail);
        internal readonly bool? _Debug_Email  = GetLogTypeEnabled(configuration.LogType.Debug.SendEmail);
        internal readonly bool? _Info_Email   = GetLogTypeEnabled(configuration.LogType.Info.SendEmail);
        internal readonly bool? _Notice_Email = GetLogTypeEnabled(configuration.LogType.Notice.SendEmail);
        internal readonly bool? _Warn_Email   = GetLogTypeEnabled(configuration.LogType.Warn.SendEmail);
        internal readonly bool? _Error_Email  = GetLogTypeEnabled(configuration.LogType.Error.SendEmail);
        internal readonly bool? _Fatal_Email  = GetLogTypeEnabled(configuration.LogType.Fatal.SendEmail);

        private readonly Dictionary<LogType, bool?> _sendEmailMap = new Dictionary<LogType, bool?>()
        {
            { LogType.Trace, configuration.LogType.Trace.SendEmail},
            { LogType.Debug, configuration.LogType.Debug.SendEmail},
            { LogType.Info, configuration.LogType.Info.SendEmail },
            { LogType.Notice, configuration.LogType.Notice.SendEmail },
            { LogType.Warn, configuration.LogType.Warn.SendEmail },
            { LogType.Error, configuration.LogType.Error.SendEmail },
            { LogType.Fatal, configuration.LogType.Fatal.SendEmail }
        };
        /// <summary>
        /// Converts message type from enum to string.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal string MessageType(LogType logType) => logType.ToLabel();

        /// <summary>
        /// Distributes what type of save is it configured. File, Email of Database.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="sendEmail"></param>
        /// <returns></returns>
        internal async Task Save(
            string message, 
            LogType logType, 
            bool? sendEmail             = true)
        {
            try
            {
                //  Send Email
                if (ShouldSendEmail(sendEmail, logType))
                    await SendEmail(sendEmail, logType, message);
            }
            catch(Exception ex)
            {
                //await SaveSimpLogError(ex.Message);
                //Dispose();
            }
        }

        /// <summary>
        /// Checks the configurations for sending Email.
        /// </summary>
        /// <param name="sendEmail"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ShouldSendEmail(bool? sendEmail, LogType logType)
        {
            //  Check for global disable or for a specific log disable sending email. Also if Email_From or Email_To are empty
            //  there is no point of continue
            if( string.IsNullOrEmpty(configuration.Email_Configuration.Email_From) || 
                string.IsNullOrEmpty(configuration.Email_Configuration.Email_To) ||
                sendEmail is false || 
                (configuration.Email_Configuration.SendEmail_Globally is not null && configuration.Email_Configuration.SendEmail_Globally is false))
                return false;

            // Check if this log type is enabled for email
            return _sendEmailMap.TryGetValue(logType, out var isEnabled) && isEnabled == true;
        }

        /// <summary>
        /// Main logic for sending Email
        /// </summary>
        /// <param name="sendEmail"></param>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal async Task<bool> SendEmail(bool? sendEmail, LogType logType, string message)
        {
            //  Check if needs to send email notification
            if ((sendEmail is true || ToSendMail(logType)) && configuration.Email_Configuration.SendEmail_Globally is true)
                EmailServices.EmailService.SendMail(DateTime.UtcNow.ToString(DateFormat) + Separator + MessageType(logType) + Separator + message);
            else
                return false;

            return true;
        }

        /// <summary>
        /// Gets from configuration for every log type configuration for sending email. Is it enabled or disabled
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ToSendMail(LogType logType) => _sendEmailMap.TryGetValue(logType, out var value) && value == true;
    }
}
