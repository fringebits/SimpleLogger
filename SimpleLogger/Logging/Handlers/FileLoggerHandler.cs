using System;
using System.IO;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class FileLoggerHandler : ILoggerHandler, IDisposable
    {
        private readonly string fileName;
        private readonly string directory;
        private readonly ILoggerFormatter loggerFormatter;

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
        }

        public void Publish(LogMessage logMessage)
        {
            if (!string.IsNullOrEmpty(this.directory))
            {
                var di = new DirectoryInfo(this.directory);
                if (!di.Exists)
                {
                    di.Create();
                }
            }

            var line = this.loggerFormatter.ApplyFormat(logMessage);
            this.WriteLine(line);
        }

        private readonly object fileLock = new object();

        private void WriteLine(string line)
        {
            lock(this.fileLock)
            {
                //TODO: Need to make this thread-safe.  Create a shared queue of 'lines to write' and start a writer thread.
                using (var writer = new StreamWriter(File.Open(this.Fullpath, FileMode.Append)))
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static string CreateFileName()
        {
            var currentDate = DateTime.Now;
            var guid = Guid.NewGuid();
            return string.Format("Log_{0:0000}{1:00}{2:00}-{3:00}{4:00}_{5}.log", currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, guid);
        }

        public void Dispose()
        {
        }
    }
}
