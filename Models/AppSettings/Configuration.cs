using System.Text.Json.Serialization;

namespace SimpLog.Email.Models.AppSettings
{
    internal class Configuration
    {
        [JsonPropertyName("Email_Configuration")]
        public EmailConfiguration? Email_Configuration { get; set;}

        [JsonPropertyName("LogType")]
        public Log? LogType { get; set; }
    }
}
