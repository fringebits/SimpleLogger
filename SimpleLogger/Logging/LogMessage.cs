namespace SimpleLogger.Logging
{
    using System;
    using System.Threading;
    using Formatters;

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
        public int IndentLevel { get; set; }

        public LogMessage() { }

        public LogMessage(Logger.Level level, string text, DateTime dateTime, string callingClass, string callingMethod, string fileName, int lineNumber)
        {
            this.Level = level;
            this.Text = text;
            this.DateTime = dateTime;
            this.CallingClass = callingClass;
            this.CallingMethod = callingMethod;
            this.Filename = fileName;
            this.LineNumber = lineNumber;
            this.ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public string Indented => $"{new string(' ', this.IndentLevel * 4)}{this.Text}";

        public override string ToString()
        {
            return new DefaultLoggerFormatter().ApplyFormat(this);
        }
    }
}
