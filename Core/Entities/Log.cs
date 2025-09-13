namespace salud_vital_proyecto1.Core.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public LogLevelType Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Source { get; set; }
    }

    public enum LogLevelType
    {
        Info,
        Warning,
        Error,
        Critical
    }
}
