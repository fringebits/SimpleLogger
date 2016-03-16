using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleLogger.Tests
{
    using System.IO;
    using System.Threading.Tasks;
    using SimpleLogger.Logging.Handlers;

    [TestClass]
    public class TestBasicLogging
    {
        private const string LogFilename = "TestBasicLogging.txt";

        private TestHandler testHandler;

        private FileLoggerHandler fileHandler;

        [TestInitialize]
        public void Initialize()
        {
            // Delete existing filename (if it exists)
            File.Delete(LogFilename);

            this.testHandler = new TestHandler();
            this.fileHandler = new FileLoggerHandler(LogFilename);

            Logger.LoggerHandlerManager
                .AddHandler(this.fileHandler)
                .AddHandler(this.testHandler)
                .AddHandler(new DebugConsoleLoggerHandler());
        }
        
        [TestMethod]
        public void CanCreateInitialLogFile()
        {
            Logger.Log();

            Assert.AreEqual(LogFilename, this.fileHandler.Fullpath);
            Assert.IsTrue(File.Exists(this.fileHandler.Fullpath));
            Assert.AreEqual(1, this.testHandler.Count);
        }

        private void LogSomeLines(int count)
        {
            for (var ii = 0; ii < count; ii++)
            {
                Logger.Log($"LogSomeLines {ii}.");
            }
        }

        private Task ExerciseLogger(int count)
        {
            return new Task(() => { this.LogSomeLines(count); });
        }

        [TestMethod]
        public void CanLogFromSingleTask()
        {
            const int Count = 10;

            this.testHandler.Reset();

            var task = this.ExerciseLogger(Count);
            task.Start();
            task.Wait();

            Assert.AreEqual(Count, this.testHandler.Count);
        }

        [TestMethod]
        public void CanLogFromMultipleTasks()
        {
            const int Count = 100;

            this.testHandler.Reset();

            var A = this.ExerciseLogger(Count);
            var B = this.ExerciseLogger(Count);
            A.Start();
            B.Start();

            Task.WaitAll(A, B);
         
            Assert.AreEqual(2 * Count, this.testHandler.Count);
        }
    }
}
