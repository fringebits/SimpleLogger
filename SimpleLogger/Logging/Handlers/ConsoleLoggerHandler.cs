using System;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    public class ConsoleLoggerHandler : ILoggerHandler
    {
        private readonly ILoggerFormatter loggerFormatter;

        private readonly bool rawOutput;

        public Logger.Level Level { get; set; } = Logger.Level.Info;

        public ConsoleLoggerHandler(bool rawOutput = true) 
            : this(new DefaultLoggerFormatter())
        {
            this.rawOutput = rawOutput;
        }

        public ConsoleLoggerHandler(ILoggerFormatter loggerFormatter)
        {
            this.loggerFormatter = loggerFormatter;
        }

        public void Publish(LogMessage logMessage)
        {
            if (logMessage.Level >= this.Level || (Logger.IsVerbose && logMessage.Level == Logger.Level.Debug))
            {
                var msg = this.rawOutput ? logMessage.Text : this.loggerFormatter.ApplyFormat(logMessage);

                Console.WriteLine(msg);
            }
        }
    }
}
