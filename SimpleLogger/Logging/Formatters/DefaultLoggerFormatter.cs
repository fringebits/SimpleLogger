namespace SimpleLogger.Logging.Formatters
{
    internal class DefaultLoggerFormatter : ILoggerFormatter
    {
        public string ApplyFormat(LogMessage logMessage)
        {
            return $"{logMessage.DateTime:yyyy.MM.dd HH:mm:ss.fff}: {logMessage.ThreadId} {logMessage.Level} [line: {logMessage.LineNumber} {logMessage.CallingClass}.{logMessage.CallingMethod}()]: {logMessage.Text}";
        }
    }
}