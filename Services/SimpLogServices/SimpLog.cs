using SimpLog.Email.Models;
using SimpLog.Email.Services.FileServices;
using System.Threading.Tasks;

namespace SimpLog.Email.Services.SimpLogServices
{
    public class SimpLog
    {
        private FileService _fileService = new FileService();

        /// <summary>
        /// If there is no configuration set up in appsettings.json, log is enabled. If there is disabled from the
        /// configuration, take it in mind here.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sendEmail"></param>
        /// <returns></returns>
        public async Task Trace(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Trace, sendEmail);

        public async Task Debug(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Debug, sendEmail);

        public async Task Info(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Info, sendEmail);
        
        public async Task Notice(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Notice, sendEmail);

        public async Task Warn(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Warn, sendEmail);

        public async Task Error(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Error, sendEmail);

        public async Task Fatal(string message, bool? sendEmail = true)
            => await _fileService.Save(message, LogType.Fatal, sendEmail);

        public async Task SendEmail(string message, LogType logType, bool? sendEmail = true)
            => await _fileService.Save(message, logType, sendEmail);
    }
}
