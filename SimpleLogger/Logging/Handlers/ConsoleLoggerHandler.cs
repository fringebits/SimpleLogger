using System;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    public class ConsoleLoggerHandler : ILoggerHandler
    {
        private readonly ILoggerFormatter loggerFormatter;

        private readonly bool rawOutput;

        public Logger.Level Level { get; set; } = Logger.Level.Info;

        public ConsoleColor TextColor { get; set; }

        public ConsoleLoggerHandler(bool rawOutput = true) 
            : this(new DefaultLoggerFormatter())
        {
            this.TextColor = Console.ForegroundColor;
            this.rawOutput = rawOutput;
        }

        public ConsoleLoggerHandler(ILoggerFormatter loggerFormatter)
        {
            this.TextColor = Console.ForegroundColor;
            this.loggerFormatter = loggerFormatter;
        }

        public void Publish(LogMessage logMessage)
        {
            if (logMessage.Level >= this.Level || (Logger.IsVerbose && logMessage.Level == Logger.Level.Debug))
            {
                var msg = this.rawOutput ? logMessage.Text : this.loggerFormatter.ApplyFormat(logMessage);

                var originalColor = Console.ForegroundColor;
                var messageColor = GetMessageColor(logMessage);

                if (messageColor != originalColor)
                {
                    Console.ForegroundColor = logMessage.ForegroundColor;
                }

                Console.WriteLine(msg);

                Console.ForegroundColor = originalColor; // most cases, this will be no-change
            }
        }

        private ConsoleColor GetMessageColor(LogMessage message)
        {
            switch (message.Level)
            {
                case Logger.Level.Error:
                case Logger.Level.Severe:
                    return ConsoleColor.Red;

                case Logger.Level.Warning:
                    return ConsoleColor.Yellow;

                default:
                    return message.ForegroundColor;
            }
        }
    }
}
