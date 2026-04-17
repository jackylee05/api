using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Threading.Tasks;

namespace Hichain.Common.Utilities
{
    public class LogHelper
    {
        /// <summary>
        /// Defines the log.
        /// </summary>
        private static readonly Logger log = LogManager.GetLogger(string.Empty);

        /// <summary>
        /// The Trace.
        /// </summary>
        /// <param name="msg">The msg<see cref="object"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Trace(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Trace(msg.ParseToString());
            }
            else
            {
                log.Trace(msg + GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="msg">The msg<see cref="object"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Debug(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Debug(msg.ParseToString());
            }
            else
            {
                log.Debug(msg + GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="msg">The msg<see cref="object"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Info(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Info(msg.ParseToString());
            }
            else
            {
                log.Info(msg + GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="msg">The msg<see cref="object"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Warn(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Warn(msg.ParseToString());
            }
            else
            {
                log.Warn(msg + GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="msg">The msg<see cref="object"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Error(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Error(msg.ParseToString());
            }
            else
            {
                log.Error(msg + GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Error(Exception ex)
        {
            if (ex != null)
            {
                log.Error(GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="msg">The msg<see cref="object"/>.</param>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Fatal(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Fatal(msg.ParseToString());
            }
            else
            {
                log.Fatal(msg + GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void Fatal(Exception ex)
        {
            if (ex != null)
            {
                log.Fatal(GetExceptionMessage(ex));
            }
        }

        /// <summary>
        /// The GetExceptionMessage.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string GetExceptionMessage(Exception ex)
        {
            string message = string.Empty;
            if (ex != null)
            {
                message += ex.Message;
                message += Environment.NewLine;
                Exception originalException = ex.GetOriginalException();
                if (originalException != null)
                {
                    if (originalException.Message != ex.Message)
                    {
                        message += originalException.Message;
                        message += Environment.NewLine;
                    }
                }
                message += ex.StackTrace;
                message += Environment.NewLine;
            }
            return message;
        }
    }
}
