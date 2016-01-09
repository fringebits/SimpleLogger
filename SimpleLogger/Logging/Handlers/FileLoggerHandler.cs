using System;
using System.IO;
using SimpleLogger.Logging.Formatters;

namespace SimpleLogger.Logging.Handlers
{
    public class FileLoggerHandler : ILoggerHandler
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
                var directoryInfo = new DirectoryInfo(Path.Combine(this.directory));
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
            }

            using (var writer = new StreamWriter(File.Open(Path.Combine(this.directory, this.fileName), FileMode.Append)))
                writer.WriteLine(this.loggerFormatter.ApplyFormat(logMessage));
        }

        private static string CreateFileName()
        {
            var currentDate = DateTime.Now;
            var guid = Guid.NewGuid();
            return string.Format("Log_{0:0000}{1:00}{2:00}-{3:00}{4:00}_{5}.log", currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, guid);
        }
    }
}
