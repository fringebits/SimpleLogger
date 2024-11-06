using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SimpleLogger.Logging;
using SimpleLogger.Logging.Handlers;
using SimpleLogger.Logging.Module;

namespace SimpleLogger
{
    public static class Logger
    {
        internal static readonly LogPublisher LogPublisher;

        private static readonly object Sync = new object();

        private static bool _isTurned = true;

        private static bool _isTurnedDebug = false;

        public enum Level
        {
            None,
            Debug,
            Info,
            Warning,
            Error,
            Severe
        }

        static Logger()
        {
            lock (Sync)
            {
                LogPublisher = new LogPublisher();
                Modules = new ModuleManager();
                Debug = new DebugLogger();
                DefaultColor = Console.ForegroundColor; // set the default to 'current color'
            }
        }

        public static void DefaultInitialization()
        {
            LoggerHandlerManager
                .AddHandler(new ConsoleLoggerHandler())
                .AddHandler(new FileLoggerHandler());

            Log(Level.Info, "Default initialization");
        }

        public static ConsoleColor DefaultColor { get; set; } 

        public static Level DefaultLevel { get; set; } = Level.Info;

        public static int IndentLevel { get; internal set; }

        public static bool IsVerbose => _isTurnedDebug;

        public static ILoggerHandlerManager LoggerHandlerManager => LogPublisher;

        public static void Log()
        {
            Log("There is no message");
        }

        public static void PushIndent()
        {
            IndentLevel += 1;
        }

        public static void PopIndent()
        {
            IndentLevel = Math.Max(0, IndentLevel - 1);
        }

        public static void Log(string message)
        {
            Log(DefaultLevel, message);
        }

        public static void Log(string message, ConsoleColor color)
        {
            Log(DefaultLevel, message, color);
        }

        public static void Log(Level level, string message, ConsoleColor? color = null)
        {
            var stackFrame = FindStackFrame();
            var methodBase = GetCallingMethodBase(stackFrame);
            var callingMethod = methodBase.Name;
            var callingClass = methodBase.ReflectedType.Name;
            var fileName = stackFrame.GetFileName();
            var lineNumber = stackFrame.GetFileLineNumber();

            Log(level, message, callingClass, callingMethod, fileName, lineNumber, color ?? DefaultColor);
        }

        public static void LogDebug(string message)
        {
            Log(Level.Debug, message, DefaultColor);
        }

        public static void LogWarning(string message)
        {
            Log(Level.Debug, message, ConsoleColor.Yellow);
        }

        public static void LogError(string message)
        {
            Log(Level.Error, message, ConsoleColor.Yellow);
        }

        public static void Log(Exception exception)
        {
            LogError(exception.Message);
            Modules.ExceptionLog(exception);
        }

        public static void Log<TClass>(Exception exception) where TClass : class
        {
            var message = $"Log exception -> Message: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log<TClass>(Level.Error, message);
        }

        public static void Log<TClass>(string message) where TClass : class
        {
            Log<TClass>(DefaultLevel, message);
        }

        public static void Log<TClass>(Level level, string message) where TClass : class
        {
            var stackFrame = FindStackFrame();
            var methodBase = GetCallingMethodBase(stackFrame);
            var callingMethod = methodBase.Name;
            var callingClass = typeof(TClass).Name;
            var fileName = stackFrame.GetFileName();
            var lineNumber = stackFrame.GetFileLineNumber();

            Log(level, message, callingClass, callingMethod, fileName, lineNumber, DefaultColor);
        }

        private static void Log(Level level, string message, string callingClass, string callingMethod, string? fileName, int lineNumber, ConsoleColor color)
        {
            if (!_isTurned || (!_isTurnedDebug && level == Level.Debug))
            {
                return;
            }

            var currentDateTime = DateTime.Now;

            Modules.BeforeLog();
            var logMessage = new LogMessage(level, message, currentDateTime, callingClass, callingMethod, fileName, lineNumber)
            {
                IndentLevel = IndentLevel,
                ForegroundColor = color,                
            };

            LogPublisher.Publish(logMessage);
            Modules.AfterLog(logMessage);
        }

        private static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null
                ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
        }

        private static StackFrame FindStackFrame()
        {
            var stackTrace = new StackTrace();
            for (var i = 0; i < stackTrace.GetFrames().Count(); i++)
            {
                var methodBase = stackTrace.GetFrame(i).GetMethod();
                var name = MethodBase.GetCurrentMethod().Name;
                if (!methodBase.Name.Equals("Log") && !methodBase.Name.Equals(name))
                {
                    return new StackFrame(i, true);
                }
            }
            return null;
        }

        public static void On()
        {
            _isTurned = true;
        }

        public static void Off()
        {
            _isTurned = false;
        }


        public static void DebugOn()
        {
            _isTurnedDebug = true;
        }

        public static void DebugOff()
        {
            _isTurnedDebug = false;
        }

        public static IEnumerable<LogMessage> Messages => LogPublisher.Messages;

        public static DebugLogger Debug { get; }

        public static ModuleManager Modules { get; }
    }
}
