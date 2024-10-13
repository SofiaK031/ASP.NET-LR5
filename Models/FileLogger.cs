namespace LR5.Models
{
	public class FileLogger : ILogger, IDisposable
	{
        private readonly string _filePath;
        static object _locker = new object();

        public FileLogger(string path)
        {
            _filePath = path;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Логувати тільки події рівня Error і Critical
            return logLevel == LogLevel.Error || logLevel == LogLevel.Critical;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (exception != null)
            {
                lock (_locker)
                {
                    File.AppendAllText(_filePath, $"{DateTime.Now:MM/dd/yyyy h:mm:ss tt} {formatter(state, exception)}\n");
                }
            }
        }

        public void Dispose() { }
    }
}