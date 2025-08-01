using PojistakNET.Models;

namespace PojistakNET.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _logContext;

        public LogService(ApplicationDbContext context)
        {
            _logContext = context;
        }

        public async Task LogAsync(string level, string message, string? userName)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                UserName = userName ?? "Anonymous",
                CreatedAt = DateTime.UtcNow
            };

            _logContext.LogEntries.Add(logEntry);
            await _logContext.SaveChangesAsync();
        }
    }
}