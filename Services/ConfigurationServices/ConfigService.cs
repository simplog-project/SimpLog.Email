using SimpLog.Email.Models.AppSettings;
using System;
using System.IO;
using System.Text.Json;

namespace SimpLog.Email.Services.ConfigurationServices
{
    internal static class ConfigService
    {
        #region Email Configuration Variable

        static readonly bool? _SendEmail_Globally = null;
        static readonly string? _Email_From = null;
        static readonly string? _Email_To = null;
        static readonly string? _Email_BCC = null;
        static readonly string? _Host = null;
        static readonly string? _Port = null;
        static readonly string? _Key = null;
        static readonly string? _Value = null;
        static readonly bool? _Enable_SSL = null;

        #endregion Email Configuration Variable

        #region Log Type Configuration Variable

        static readonly bool? _TraceSendEmail        = true;

        static readonly bool? _DebugSendEmail        = false;
        
        static readonly bool? _InfoSendEmail         = false;
        
        static readonly bool? _NoticeSendEmail       = false;
        
        static readonly bool? _WarnSendEmail         = false;
        
        static readonly bool? _ErrorSendEmail        = false;
        
        static readonly bool? _FatalSendEmail        = false;

        #endregion Log Type Configuration Variable

        static ConfigService()
        {
            Configuration? simpLogConfig;

            //  If there is not found a configuration file
            if (!File.Exists(Environment.CurrentDirectory + "\\simplog.json"))
            {
                simpLogConfig = new Configuration()
                {
                    Email_Configuration = new EmailConfiguration()
                    {
                        SendEmail_Globally = null,
                        Email_From = null,
                        Email_Bcc = null,
                        Enable_SSL = null,
                        Email_Connection = new EmailConnection()
                        {
                            API_Key = null,
                            API_Value = null,
                            Host = null,
                            Port = null,
                        },
                        Email_To = null,
                    },
                    LogType = new Log()
                    {
                        Debug = new LogTypeObject()
                        {
                            SendEmail = null
                        },
                        Error = new LogTypeObject()
                        {
                            SendEmail = null
                        },
                        Fatal = new LogTypeObject()
                        {
                            SendEmail = null
                        },
                        Info = new LogTypeObject()
                        {
                            SendEmail = null
                        },
                        Notice = new LogTypeObject()
                        {
                            SendEmail = null
                        },
                        Trace = new LogTypeObject()
                        {
                            SendEmail = null
                        },
                        Warn = new LogTypeObject()
                        {
                            SendEmail = null
                        }
                    }
                };
            }
            else
                simpLogConfig = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Environment.CurrentDirectory + "\\simplog.json"));

            #region Email Configuration Get From Json
            _SendEmail_Globally = simpLogConfig.Email_Configuration.SendEmail_Globally;
            _Email_From = simpLogConfig.Email_Configuration.Email_From;
            _Email_To = simpLogConfig.Email_Configuration.Email_To;
            _Email_BCC = simpLogConfig.Email_Configuration.Email_Bcc;
            _Host = simpLogConfig.Email_Configuration.Email_Connection.Host;
            _Port = simpLogConfig.Email_Configuration.Email_Connection.Port;
            _Key = simpLogConfig.Email_Configuration.Email_Connection.API_Key;
            _Value = simpLogConfig.Email_Configuration.Email_Connection.API_Value;
            _Enable_SSL = simpLogConfig.Email_Configuration.Enable_SSL;

            #endregion Email Configuration Get From Json

            #region Log Type Configuration Get From Json

            //  Checks if the configuration exists at all
            _TraceSendEmail = simpLogConfig.LogType.Trace.SendEmail;
            
            _DebugSendEmail = simpLogConfig.LogType.Debug.SendEmail;
            
            _InfoSendEmail = simpLogConfig.LogType.Info.SendEmail;
            
            _NoticeSendEmail = simpLogConfig.LogType.Notice.SendEmail;
            
            _WarnSendEmail = simpLogConfig.LogType.Warn.SendEmail;
            
            _ErrorSendEmail = simpLogConfig.LogType.Error.SendEmail;
            
            _FatalSendEmail = simpLogConfig.LogType.Fatal.SendEmail;
            
            #endregion Log Type Configuration Get From Json
        }

        /// <summary>
        /// Put into an object the configuration from appsettings.json file
        /// </summary>
        /// <returns></returns>
        public static Configuration BindConfigObject()
        {
            return new Configuration()
            {
                Email_Configuration     = new EmailConfiguration()
                {
                    SendEmail_Globally = _SendEmail_Globally,
                    Email_From          = _Email_From,
                    Email_To            = _Email_To,
                    Email_Bcc           = _Email_BCC,
                    Enable_SSL          = _Enable_SSL,
                    Email_Connection    = new EmailConnection()
                    {
                        Host        = _Host,
                        Port        = _Port,
                        API_Key     = _Key,
                        API_Value   = _Value,
                    }
                },
                LogType                 = new Log()
                {
                    Trace   = new LogTypeObject()
                    {
                        SendEmail       = _TraceSendEmail,
                    },
                    Debug   = new LogTypeObject()
                    {
                        SendEmail       = _DebugSendEmail,
                    },
                    Info    = new LogTypeObject()
                    {
                        SendEmail       = _InfoSendEmail,
                    },
                    Notice  = new LogTypeObject()
                    {
                        SendEmail       = _NoticeSendEmail,
                    },
                    Warn    = new LogTypeObject()
                    {
                        SendEmail       = _WarnSendEmail,
                    },
                    Error   = new LogTypeObject()
                    {
                        SendEmail       = _ErrorSendEmail,
                    },
                    Fatal   = new LogTypeObject()
                    {
                        SendEmail       = _FatalSendEmail,
                    },
                }
            };
        }
    }
}
