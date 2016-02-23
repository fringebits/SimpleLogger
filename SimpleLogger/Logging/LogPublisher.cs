using System.Collections.Generic;

namespace SimpleLogger.Logging
{
    internal class LogPublisher : ILoggerHandlerManager
    {
        private readonly IList<ILoggerHandler> loggerHandlers;
        private readonly IList<LogMessage> messages;

        public LogPublisher()
        {
            this.loggerHandlers = new List<ILoggerHandler>();
            this.messages = new List<LogMessage>();
        }

        public void Publish(LogMessage logMessage)
        {
            this.messages.Add(logMessage);
            foreach (var loggerHandler in this.loggerHandlers)
                loggerHandler.Publish(logMessage);
        }

        public ILoggerHandlerManager AddHandler(ILoggerHandler loggerHandler)
        {
            if (loggerHandler != null)
                this.loggerHandlers.Add(loggerHandler);
            return this;
        }

        public bool RemoveHandler(ILoggerHandler loggerHandler)
        {
            loggerHandler.Shutdown();
            return this.loggerHandlers.Remove(loggerHandler);
        }

        public void RemoveAll()
        {
            foreach (var h in this.loggerHandlers)
            {
                h.Shutdown();
            }
            this.loggerHandlers.Clear();
        }

        public IEnumerable<LogMessage> Messages
        {
            get { return this.messages; }
        }
    }
}
