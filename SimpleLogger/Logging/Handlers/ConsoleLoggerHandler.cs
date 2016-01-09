using System;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    public class ConsoleLoggerHandler : ILoggerHandler
    {
        private readonly ILoggerFormatter _loggerFormatter;

        private bool cleanOutput;

        public ConsoleLoggerHandler(bool cleanOutput = true) : this(new DefaultLoggerFormatter()) { this.cleanOutput = cleanOutput; }

        public ConsoleLoggerHandler(ILoggerFormatter loggerFormatter)
        {
            _loggerFormatter = loggerFormatter;
        }

        public void Publish(LogMessage logMessage)
        {
            if (cleanOutput)
            {
                Console.WriteLine(logMessage.Text);
            }
            else
            {
                Console.WriteLine(_loggerFormatter.ApplyFormat(logMessage));
            }
        }
    }
}
