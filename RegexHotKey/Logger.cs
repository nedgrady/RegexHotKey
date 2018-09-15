using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logging;
using System.Resources;
namespace RegexHotKey
{
    class Logger
    {

        #region Singleton
        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger());

        public static Logger Instance { get { return lazy.Value; } }
        #endregion

#if DEBUG
        LogLevel minLogLevel = LogLevel.Verbose;
#else
        LogLevel minLogLevel = LogLevel.Warning;
#endif

        private FileLogger _fileLogger;

        private Logger()
        {
            _fileLogger = new FileLogger(filePath: @"TEST.txt", maxLogWait: new TimeSpan(0, 0, 5), sleepInterval: new TimeSpan(0, 0, 5), minLogLevel: minLogLevel);
        }

        public async Task LogAsync(LogLevel logLevel, string @string, params string[] replacements)
        {

            await ThrowOrLogParameter(@string, "@string");
            await ThrowOrLogParameter(@string, "replacements");

            await _fileLogger.LogAsync(@string.ReplaceMany(replacements), logLevel);
        }

        public async Task LogManyAsync(LogLevel logLevel, IEnumerable<string> strings, params string[] replacements)
        {
            await ThrowOrLogParameter(strings, "strings");

            foreach(string s in strings)
            {
                await ThrowOrLogParameter(s, "strings item");
                await LogAsync(logLevel, s);
            }
        }

        private async Task ThrowOrLogParameter(object parameter, string parameterName)
        {
            if(parameter == null)
            {
#if DEBUG
            throw new ArgumentNullException(parameterName);
#pragma warning disable CS0162 // Unreachable code detected
#endif

            await _fileLogger.LogAsync(Errors.NULL_ITEM_LOGGED.ReplaceMany(parameterName ?? ""), LogLevel.Warning);
            await _fileLogger.FlushAsync();
            return;
#if DEBUG
#pragma warning restore CS0162 // Unreachable code detected
#endif
            }
        }

        ~Logger()
        {
            _fileLogger.Dispose();
        }
    }
}
