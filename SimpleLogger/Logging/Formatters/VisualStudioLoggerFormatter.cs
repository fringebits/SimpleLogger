namespace SimpleLogger.Logging.Formatters
{
    public class VisualStudioLoggerFormatter : ILoggerFormatter
    {
        public string ApplyFormat(LogMessage logMessage)
        {
            return $"{logMessage.Filename}({logMessage.LineNumber},0): {logMessage.ThreadId}, {logMessage.Level}, {logMessage.Text}";
        }
    }
}