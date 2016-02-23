using System;
using System.IO;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class FileLoggerHandler : ILoggerHandler
    {
        private readonly string fileName;
        private readonly string directory;
        private readonly ILoggerFormatter loggerFormatter;
        private readonly List<string> logQueue; 
        private readonly Thread worker;
        private bool stopWork = false;

        public string Fullpath
        {
            get { return Path.Combine(this.directory, this.fileName); }
        }

        public FileLoggerHandler() : this(CreateFileName()) { }

        public FileLoggerHandler(string fileName) : this(fileName, string.Empty) { }

        public FileLoggerHandler(string fileName, string directory) : this(new DefaultLoggerFormatter(), fileName, directory) { }

        public FileLoggerHandler(ILoggerFormatter loggerFormatter) : this(loggerFormatter, CreateFileName()) { }

        public FileLoggerHandler(ILoggerFormatter loggerFormatter, string fileName) : this(loggerFormatter, fileName, string.Empty) { }

        public FileLoggerHandler(ILoggerFormatter loggerFormatter, string fileName, string directory)
        {
            this.loggerFormatter = loggerFormatter;
            this.fileName = string.IsNullOrEmpty(fileName) ? CreateFileName() : fileName;
            this.directory = directory;
            this.logQueue = new List<string>();
            this.worker = new Thread(this.Worker);
            this.worker.Start();
        }

        public void Shutdown()
        {
            this.stopWork = true;
            this.worker.Join();
        }

        public void Publish(LogMessage logMessage)
        {
            if (!string.IsNullOrEmpty(this.directory))
            {
                var directoryInfo = new DirectoryInfo(Path.Combine(this.directory));
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
            }

            var line = this.loggerFormatter.ApplyFormat(logMessage);
            this.logQueue.Add(line);
        }

        private void Worker()
        {
            while (!this.stopWork)
            {
                if (this.logQueue.Any())
                {
                    //TODO: Need to make this thread-safe.  Create a shared queue of 'lines to write' and start a writer thread.
                    using (var writer = new StreamWriter(File.Open(Path.Combine(this.directory, this.fileName), FileMode.Append)))
                        writer.WriteLine(this.logQueue.First());
                    this.logQueue.RemoveAt(0);
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private static string CreateFileName()
        {
            var currentDate = DateTime.Now;
            var guid = Guid.NewGuid();
            return string.Format("Log_{0:0000}{1:00}{2:00}-{3:00}{4:00}_{5}.log", currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, guid);
        }
    }
}
