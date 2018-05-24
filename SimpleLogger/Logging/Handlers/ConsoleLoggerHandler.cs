using System;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    public class ConsoleLoggerHandler : ILoggerHandler
    {
        private readonly ILoggerFormatter loggerFormatter;

        private readonly bool cleanOutput;

        public Logger.Level Level { get; set; } = Logger.Level.Info;

        public ConsoleLoggerHandler(bool cleanOutput = true) 
            : this(new DefaultLoggerFormatter())
        {
            this.cleanOutput = cleanOutput;
        }

        public ConsoleLoggerHandler(ILoggerFormatter loggerFormatter)
        {
            this.loggerFormatter = loggerFormatter;
        }

        public void Publish(LogMessage logMessage)
        {
            if (logMessage.Level < this.Level)
            {
                return;
            }

            if (this.cleanOutput)
            {
                Console.WriteLine(logMessage.Text);
            }
            else
            {
                Console.WriteLine(this.loggerFormatter.ApplyFormat(logMessage));
            }
        }
    }
}
