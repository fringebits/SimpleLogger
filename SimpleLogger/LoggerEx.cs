namespace SimpleLogger
{
    using System;
    using System.IO;
    using System.Reflection;
    using Logging.Handlers;

    // TODO: Provide option to cleanup/prune 'old' logs (?)

    public static class LoggerEx
    {
        private static string GetAppDataFolder(string appName = null)
        {
            if (string.IsNullOrEmpty(appName))
            {
                var assembly = Assembly.GetExecutingAssembly();
                appName = assembly.GetName().Name;
            }

            var rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(rootFolder, appName);
        }

        public static void InitalizeWfpLogger(string appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler());
        }

        public static void InitializeConsoleLogger(string appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler())
                .AddHandler(new ConsoleLoggerHandler());

        }

        public static void InitializeUnitTestLogger(string appName = null)
        {
            var h = new FileLoggerHandler(string.Empty, GetAppDataFolder(appName));

            Logger.LoggerHandlerManager
                .AddHandler(h)
                .AddHandler(new DebugConsoleLoggerHandler());
        }
    }
}