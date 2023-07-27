using System;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Core.Tests
{
    public abstract class AbstractLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
            =>  new LoggingScope();

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => Log(logLevel, exception, formatter(state, exception));

        public abstract void Log(LogLevel logLevel, Exception ex, string information);

        public class LoggingScope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
