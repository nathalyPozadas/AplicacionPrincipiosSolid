using salud_vital_proyecto1.Core.Entities;
using salud_vital_proyecto1.Core.Interfaces;
using salud_vital_proyecto1.Infraestructure.Data;

namespace salud_vital_proyecto1.Core.Services
{
    public class LogService : ILogService 
    {
        private readonly ApplicationDbContext _context;

        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string message, LogLevelType level)
        {
            var log = new Log
            {
                Message = message,
                Level = level,
                Source = "Application",
                CreatedAt = DateTime.UtcNow
            };
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
