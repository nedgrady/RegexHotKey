using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DontThink.Utilities.Logging;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RegexHotKey
{
    public class Logger
    {

        #region Singleton
        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger());

        public static Logger Instance { get { return lazy.Value; } }
        #endregion

        protected LogLevel _logStackTraceThreshold = LogLevel.Information;

#if DEBUG
        protected LogLevel _minLogLevel = LogLevel.Verbose;
#else
        protected LogLevel _minLogLevel = LogLevel.Warning;
#endif

        private FileLogger _fileLogger;

        private Logger()
        {
            _fileLogger = new FileLogger(
                filePath: @"RegexHotkeysLogs.txt",
                maxLogWait: new TimeSpan(0, 0, 5),
                sleepInterval: new TimeSpan(0, 0, 5),
                minLogLevel: _minLogLevel);
        }

        public async Task LogAsync(LogLevel logLevel, string @string, params string[] replacements)
        {
#if DEBUG
#if !VERBOSE
            if (logLevel > LogLevel.Information)
#endif
                Console.WriteLine(@string.ReplaceMany(replacements));
#endif
            await ThrowOrLogNullLogEntry(@string, "@string");
            await ThrowOrLogNullLogEntry(@string, "replacements");
            await _fileLogger.LogAsync(@string.ReplaceMany(replacements), logLevel);
        }

        public async Task LogManyAsync(LogLevel logLevel, IEnumerable<string> strings, params string[] replacements)
        {
            await ThrowOrLogNullLogEntry(strings, "strings");

            foreach(string s in strings)
            {
                await ThrowOrLogNullLogEntry(s, "strings item");
                await LogAsync(logLevel, s);
            }
        }

        public async Task LogManyAsync(LogLevel logLevel, params string[] strings)
        {
            await LogManyAsync(logLevel, (IEnumerable<string>)strings);
        }

        public async Task ThrowOrLogNullLogEntry(object parameter, string paramName)
        {
            if(parameter == null)
            {
#if DEBUG
                throw new ArgumentNullException(paramName);
#pragma warning disable CS0162 // Unreachable code detected
#endif

                await LogAsync(LogLevel.Warning, Errors.NULL_ITEM_LOGGED, paramName ?? "");
                await _fileLogger.FlushAsync();
                return;
#if DEBUG
#pragma warning restore CS0162 // Unreachable code detected
#endif
            }
        }

        public async Task ThrowOrLogException(Exception ex, LogLevel logLevel)
        {
#if DEBUG
            throw (ex ?? new ArgumentException("ex"));
#endif
            await LogAsync(logLevel, ex.StackTrace);
        }

        public async Task ThrowOrLogNullArgument(object parameter, string paramName, LogLevel logLevel = LogLevel.Error)
        {
            if (parameter == null)
            {
                await ThrowOrLogException(new ArgumentNullException(paramName ?? ""), logLevel);
            }
        }

        ~Logger()
        {
            _fileLogger.Dispose();
        }
    }
}
