using System.Text.Json.Serialization;

namespace SimpLog.Email.Models.AppSettings
{
    internal class LogTypeObject
    {
        [JsonPropertyName("SendEmail")]
        public bool? SendEmail { get; set; }
    }
}
