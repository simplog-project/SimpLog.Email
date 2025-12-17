using System.Text.Json.Serialization;

namespace SimpLog.Email.Models.AppSettings
{
    internal class EmailConfiguration
    {
        [JsonPropertyName("SendEmail_Globally")]
        public bool? SendEmail_Globally { get; set; }

        [JsonPropertyName("Email_From")]
        public string? Email_From { get; set; }

        [JsonPropertyName("Email_To")]
        public string? Email_To { get; set; }

        [JsonPropertyName("Email_Bcc")]
        public string? Email_Bcc { get; set; }

        [JsonPropertyName("Enable_SSL")]
        public bool? Enable_SSL { get; set; }

        [JsonPropertyName("Email_Connection")]
        public EmailConnection? Email_Connection { get; set; }
    }
}
