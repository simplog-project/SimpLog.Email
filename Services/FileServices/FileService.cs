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

        internal readonly bool? _Trace_Email  = (configuration.LogType.Trace.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Trace.SendEmail);
        internal readonly bool? _Debug_Email  = (configuration.LogType.Debug.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Debug.SendEmail);
        internal readonly bool? _Info_Email   = (configuration.LogType.Info.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Info.SendEmail);
        internal readonly bool? _Notice_Email = (configuration.LogType.Notice.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Notice.SendEmail);
        internal readonly bool? _Warn_Email   = (configuration.LogType.Warn.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Warn.SendEmail);
        internal readonly bool? _Error_Email  = (configuration.LogType.Error.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Error.SendEmail);
        internal readonly bool? _Fatal_Email  = (configuration.LogType.Fatal.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Fatal.SendEmail);

        /// <summary>
        /// Converts message type from enum to string.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal string MessageType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Trace:
                    return LogType_Trace;
                case LogType.Debug:
                    return LogType_Debug;
                case LogType.Info:
                    return LogType_Info;
                case LogType.Notice:
                    return LogType_Notice;
                case LogType.Warn:
                    return LogType_Warn;
                case LogType.Error:
                    return LogType_Error;
                case LogType.Fatal:
                    return LogType_Fatal;
                default:
                    return LogType_NoType;
            }
        }

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

            switch (logType)
            {
                case LogType.Trace:
                    {
                        if (_Trace_Email is not null && _Trace_Email is false)
                            return false;
                        break;
                    }
                case LogType.Debug:
                    {
                        if (_Debug_Email is not null && _Debug_Email is false)
                            return false;
                        break;
                    }
                case LogType.Info:
                    {
                        if (_Info_Email is not null && _Info_Email is false)
                            return false;
                        break;
                    }
                case LogType.Notice:
                    {
                        if (_Notice_Email is not null && _Notice_Email is false)
                            return false;
                        break;
                    }
                case LogType.Warn:
                    {
                        if (_Warn_Email is not null && _Warn_Email is false)
                            return false;
                        break;
                    }
                case LogType.Error:
                    {
                        if (_Error_Email is not null && _Error_Email is false)
                            return false;
                        break;
                    }
                case LogType.Fatal:
                    {
                        if (_Fatal_Email is not null && _Fatal_Email is false)
                            return false;
                        break;
                    }
            }

            return true;
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
        internal bool ToSendMail(LogType logType)
        {
            switch(logType)
            { 
                case LogType.Trace:
                    return configuration.LogType.Trace.SendEmail.Value;
                case LogType.Debug:
                    return configuration.LogType.Debug.SendEmail.Value;
                case LogType.Info:
                    return configuration.LogType.Info.SendEmail.Value;
                case LogType.Notice:
                    return configuration.LogType.Notice.SendEmail.Value;
                case LogType.Warn:
                    return configuration.LogType.Warn.SendEmail.Value;
                case LogType.Error:
                    return configuration.LogType.Error.SendEmail.Value;
                case LogType.Fatal:
                    return configuration.LogType.Fatal.SendEmail.Value;
                default:
                    return false;
            }
        }
    }
}
