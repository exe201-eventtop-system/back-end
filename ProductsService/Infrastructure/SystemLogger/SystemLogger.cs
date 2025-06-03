using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SystemLogger
{
    public class SystemLogger : ILogger
    {
        private readonly SystemLoggerProvider _provider;

        public SystemLogger([NotNull] SystemLoggerProvider provider)
        {
            _provider = provider;
        }

        private string ConvertLogLevelToTxtString(LogLevel logLevel)
        {
            string result = logLevel switch
            {
                LogLevel.Critical => "CRITICAL",
                LogLevel.Warning => "WARNING",
                LogLevel.Information => "INFORMATION",
                LogLevel.Debug => "DEBUG",
                LogLevel.Trace => "TRACE",
                LogLevel.Error => "ERROR",
                _ => logLevel.ToString().ToUpper(),
            };

            return result;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Don't log anything to the dataabse if the logger is not enabled.
            if (!IsEnabled(logLevel))
            {
                return;
            }

            // validation
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);
            string logLevelText = ConvertLogLevelToTxtString(logLevel);

            Console.WriteLine(message);
        }
    }
}
