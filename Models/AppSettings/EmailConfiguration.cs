namespace SimpLog.Email.Models.AppSettings
{
    internal class EmailConfiguration
    {
        public bool? SendEmail_Globally { get; set; }

        public string? Email_From { get; set; }

        public string? Email_To { get; set; }

        public string? Email_Bcc { get; set; }

        public bool? Enable_SSL { get; set; }

        public EmailConnection? Email_Connection { get; set; }
    }
}
