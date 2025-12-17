using System.Text.Json.Serialization;

namespace SimpLog.Email.Models.AppSettings
{
    internal class EmailConnection
    {
        [JsonPropertyName("Host")]
        public string? Host { get; set; }

        [JsonPropertyName("Port")]
        public string? Port { get; set; }

        [JsonPropertyName("API_Key")]
        public string? API_Key { get; set; }

        [JsonPropertyName("API_Value")]
        public string? API_Value { get; set; }
    }
}
