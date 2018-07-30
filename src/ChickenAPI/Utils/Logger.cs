using System;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace ChickenAPI.Utils
{
    public class Logger
    {
        private const string DefaultLayout = "[${date}][${level:uppercase=true}][${logger:shortName=true}] ${message} ${exception:format=tostring}";
        
        private Logger(Type type) => Log = LogManager.GetLogger(type.ToString());

        private ILogger Log { get; }

        /// <summary>
        ///     Initialize logger's configuration.
        ///     Please refer to https://github.com/nlog/NLog/wiki/Layout-Renderers for custom layouts.
        /// </summary>
        /// <param name="consoleLayout"></param>
        /// <param name="fileLayout"></param>
        public static void Initialize(string consoleLayout, string fileLayout)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            var fileTarget = new FileTarget();

            consoleTarget.Layout = consoleLayout;

            var infoHighlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Info"),
                ForegroundColor = ConsoleOutputColor.Green
            };
            var errorHighlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Error"),
                ForegroundColor = ConsoleOutputColor.Red
            };
            var warnHighlightingRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Warn"),
                ForegroundColor = ConsoleOutputColor.DarkYellow
            };
            consoleTarget.RowHighlightingRules.Add(infoHighlightRule);
            consoleTarget.RowHighlightingRules.Add(errorHighlightRule);
            consoleTarget.RowHighlightingRules.Add(warnHighlightingRule);

            fileTarget.Layout = fileLayout;
            fileTarget.FileName = "logs/" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".log";

            config.AddTarget("console", consoleTarget);
            config.AddTarget("file", fileTarget);

            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
        }

        public static void Initialize()
        {
            Initialize(DefaultLayout, DefaultLayout);
        }

        public static Logger GetLogger<TClass>()
        where TClass : class => new Logger(typeof(TClass));

        public void Trace(string msg)
        {
            Log?.Trace(msg);
        }

        public void Debug(string msg)
        {
            Log?.Debug(msg);
        }

        public void Info(string msg)
        {
            Log?.Info(msg);
        }

        public void Warn(string msg)
        {
            Log?.Warn(msg);
        }

        public void Error(string msg, Exception ex)
        {
            Log?.Error(ex, msg);
        }

        public void Fatal(string msg, Exception ex)
        {
            Log?.Fatal(ex, msg);
        }
    }
}