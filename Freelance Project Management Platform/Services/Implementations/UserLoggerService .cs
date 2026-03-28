using Freelance_Project_Management_Platform.Services.Interfaces;
using Serilog.Context;

namespace StudentCourseManagement.Services.Implementations
{
    public class UserLoggerService : IUserLoggerService
    {
        private readonly ILogger<UserLoggerService> _logger;

        public UserLoggerService(ILogger<UserLoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(User? user, string message, params object[] args)
        {
            Log(user, LogLevel.Information, null, message, args);
        }

        public void LogWarning(User? user, string message, params object[] args)
        {
            Log(user, LogLevel.Warning, null, message, args);
        }

        public void LogError(User? user, Exception? ex, string message, params object[] args)
        {
            Log(user, LogLevel.Error, ex, message, args);
        }

        private void Log(User? user, LogLevel level, Exception? ex, string message, params object[] args)
        {
            if (user != null)
            {
                using (LogContext.PushProperty("UserId", user.Id))
                using (LogContext.PushProperty("Email", user.Email))
                using (LogContext.PushProperty("Role", user.Role.ToString()))
                {
                    WriteLog(level, ex, message, args);
                }
            }
            else
            {
                WriteLog(level, ex, message, args);
            }
        }

        private void WriteLog(LogLevel level, Exception? ex, string message, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Information:
                    _logger.LogInformation(message, args);
                    break;

                case LogLevel.Warning:
                    _logger.LogWarning(message, args);
                    break;

                case LogLevel.Error:
                    if (ex != null)
                        _logger.LogError(ex, message, args);
                    else
                        _logger.LogError(message, args);
                    break;
            }
        }
    }
}