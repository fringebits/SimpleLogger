namespace SimpleLogger.Tests
{
    using SimpleLogger.Logging;

    public class TestHandler 
        : ILoggerHandler
    {
        public int Count { get; private set; }

        public void Reset()
        {
            this.Count = 0;
        }

        public void Publish(LogMessage logMessage)
        {
            this.Count += 1;
            //throw new System.NotImplementedException();
        }
    }
}