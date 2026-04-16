using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hichain.Business.SqlServerDatabaseEF
{
    /// <summary>
    /// EfLoggerProvider
    /// </summary>
    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new EFLogger(categoryName);
        }
        public void Dispose()
        {
        }
    }
    public class EFLogger : ILogger
    {
        private readonly string categoryName;
        public EFLogger(string categoryName) => this.categoryName = categoryName;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command")
            {
                var logContent = formatter(state, exception);
                LogHelper.Info(logContent);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(logContent);
                Console.ResetColor();
            }
        }
        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
