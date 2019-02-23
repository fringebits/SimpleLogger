using System.Collections.Generic;

namespace SimpleLogger.Logging
{
    using System.Threading;

    internal class LogPublisher : ILoggerHandlerManager
    {
        private readonly IList<ILoggerHandler> loggerHandlers;
        private readonly IList<LogMessage> messages;
        private readonly object logLock = new object();

        public LogPublisher()
        {
            this.loggerHandlers = new List<ILoggerHandler>();
            this.messages = new List<LogMessage>();
        }

        public void Publish(LogMessage logMessage)
        {
            if (Monitor.TryEnter(this.logLock, 50))
            {
                try
                {
                    this.messages.Add(logMessage);
                }
                finally
                {
                    Monitor.Exit(this.logLock);
                }
            }

            if (Monitor.TryEnter(this.logLock, 50))
            {
                try
                {
                    foreach (var loggerHandler in this.loggerHandlers)
                    {
                        loggerHandler.Publish(logMessage);
                    }
                }
                finally
                {
                    Monitor.Exit(this.logLock);
                }
            }
        }

        public ILoggerHandlerManager AddHandler(ILoggerHandler loggerHandler)
        {
            if (loggerHandler != null)
            {
                this.loggerHandlers.Add(loggerHandler);
            }

            return this;
        }

        public bool RemoveHandler(ILoggerHandler loggerHandler)
        {
            return this.loggerHandlers.Remove(loggerHandler);
        }

        public void RemoveAll()
        {
            this.loggerHandlers.Clear();
        }

        public IEnumerable<LogMessage> Messages => this.messages;
    }
}
