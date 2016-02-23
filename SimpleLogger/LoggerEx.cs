namespace SimpleLogger
{
    using System;
    using System.IO;
    using System.Reflection;
    using Logging.Handlers;

    // TODO: Provide option to cleanup/prune 'old' logs from AppData folder (?)

    public static class LoggerEx
    {
        private static string GetAppDataFolder(string appName = null)
        {
            if (string.IsNullOrEmpty(appName))
            {
                var assembly = Assembly.GetEntryAssembly();
                appName = assembly.GetName().Name;
            }

            var rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(rootFolder, appName);
        }
        
        public static void Shutdown()
        {
            Logger.Log("LoggerEx.Shutdown():");
            Logger.LogPublisher.RemoveAll();
        }
        
        public static FileLoggerHandler InitalizeWfpLogger(string appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler());

            return h;
        }

        public static FileLoggerHandler InitializeConsoleLogger(string appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler())
                .AddHandler(new ConsoleLoggerHandler());

            return h;
        }

        public static FileLoggerHandler InitializeUnitTestLogger(string appName = "UnitTest")
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler());

            return h;
        }
    }
}