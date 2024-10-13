namespace LR5.Models;
public class ExceptionFileLogger : ILoggerProvider
{
    readonly string _path;

    public ExceptionFileLogger(string path)
    {
        _path = path;
    }
    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_path);
    }

    public void Dispose() { }
}

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new ExceptionFileLogger(filePath));
        return builder;
    }
}