using System;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging
{
    using System.Threading;

    public class LogMessage
    {
        public DateTime DateTime { get; set; }
        public Logger.Level Level { get; set; }
        public int ThreadId { get; set; }
        public string Filename { get; set; }
        public int LineNumber { get; set; }
        public string Text { get; set; }
        public string CallingClass { get; set; }
        public string CallingMethod { get; set; }

        public LogMessage() { }

        public LogMessage(Logger.Level level, string text, DateTime dateTime, string callingClass, string callingMethod, string fileName, int lineNumber)
        {
            Level = level;
            Text = text;
            DateTime = dateTime;
            CallingClass = callingClass;
            CallingMethod = callingMethod;
            Filename = fileName;
            LineNumber = lineNumber;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public override string ToString()
        {
            return new DefaultLoggerFormatter().ApplyFormat(this);
        }
    }
}
