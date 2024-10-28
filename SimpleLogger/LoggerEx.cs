namespace SimpleLogger
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Logging.Handlers;

    // TODO: Provide option to cleanup/prune 'old' logs from AppData folder (?)

    public static class LoggerEx
    {
        private static FileLoggerHandler fileLoggerHandler;

        public static string CurrentFileHandlerLogPath => Path.GetDirectoryName(FileLoggerHandler.Fullpath);

        private static FileLoggerHandler FileLoggerHandler
        {
            get { return fileLoggerHandler; }
            set 
            {
                Debug.Assert(fileLoggerHandler == null);
                fileLoggerHandler = value;
            }
        }

        private static string GetAppDataFolder(string? appName = null)
        {
            if (string.IsNullOrEmpty(appName))
            {
                var assembly = Assembly.GetEntryAssembly();
                appName = assembly.GetName().Name;

                var attribs = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
                if (attribs.Length > 0)
                {
                    var companyName = ((AssemblyCompanyAttribute)attribs[0]).Company;
                    if (!string.IsNullOrEmpty(companyName))
                    {
                        appName = Path.Combine(companyName, appName);
                    }
                }
            }

            var rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(rootFolder, appName);
        }
        
        public static FileLoggerHandler InitalizeWfpLogger(string? appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler());

            FileLoggerHandler = h;

            return h;
        }

        public static FileLoggerHandler InitializeConsoleLogger(string? appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler())
                .AddHandler(new ConsoleLoggerHandler());

            FileLoggerHandler = h;

            return h;
        }

        public static FileLoggerHandler InitializeUnitTestLogger(string appName = "UnitTest")
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler());

            FileLoggerHandler = h;

            return h;
        }
    }
}