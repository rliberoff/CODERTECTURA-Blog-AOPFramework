using System;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

using AOPFramework;

using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace AOPLoggingApplicationBlock
{
    /// <summary>
    /// Intercept processor for the calling and returning messages of the methods decorated as interceptable by this component.
    /// </summary>
    internal class LogProcessor : IProcessor
    {
        #region IProcessor Members

        /// <summary>
        /// Processes the call message, which happens before the execution of the body of the interceptable or processable method.
        /// </summary>
        /// <param name="callMessage">The <c>Method Call Message</c> to process.</param>
        /// <param name="processable">The processable or interceptable object.</param>
        public void ProcessCallMessage(IMethodCallMessage callMessage, InterceptableAttribute processable)
        {
            string logInfo = null;
            LogAttribute logAttribute = (LogAttribute)processable;

            switch (logAttribute.Verbosity)
            {
                case VerbosityLevel.Light:
                    logInfo = GetLiteInfoFromCallMessage(callMessage);
                    break;

                case VerbosityLevel.Medium:
                    logInfo = GetMediumInfoFromCallMessage(callMessage);
                    break;

                case VerbosityLevel.Full:
                    logInfo = GetFullInfoFromCallMessage(callMessage);
                    logAttribute.Timer.Start();
                    break;

                default:
                case VerbosityLevel.None:
                    // Do nothing.
                    break;
            }

            if (!string.IsNullOrEmpty(logInfo))
            {
                WriteLog(logInfo);
            }
        }

        /// <summary>
        /// Processes the return message, which happens after the execution of the body of the interceptable or processable method.
        /// </summary>
        /// <param name="returnMessage">The <c>Method Return Message</c> to process.</param>
        /// <param name="processable">The processable or interceptable object.</param>
        public void ProcessReturnMessage(IMethodReturnMessage returnMessage, InterceptableAttribute processable)
        {
            string logInfo = null;
            LogAttribute logAttribute = (LogAttribute)processable;

            switch (logAttribute.Verbosity)
            {
                case VerbosityLevel.Light:
                    logInfo = GetLiteInfoFromReturnMessage(returnMessage);
                    break;

                case VerbosityLevel.Medium:
                    logInfo = GetMediumInfoFromReturnMessage(returnMessage);
                    break;

                case VerbosityLevel.Full:
                    logAttribute.Timer.Stop();
                    logInfo = GetFullInfoFromReturnMessage(returnMessage, logAttribute.Timer.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
                    break;

                default:
                case VerbosityLevel.None:
                    // Do nothing.
                    break;
            }

            if (!string.IsNullOrEmpty(logInfo))
            {
                WriteLog(logInfo);
            }
        }

        #endregion

        /// <summary>
        /// Writes a log info in a <c>Entry</c>.
        /// </summary>
        /// <remarks>
        /// This method leverages on the Microsoft Enterprise Library Logging Application Block.
        /// </remarks>
        /// <param name="logInfo">The log info to write.</param>
        private static void WriteLog(string logInfo)
        {
            Logger.Write(new LogEntry { Message = logInfo });
        }

        #region Info from IMethodCallMessage

        /// <summary>
        /// Gets lite info from the <c>Method Call Message</c>.
        /// </summary>
        /// <param name="message">The <c>Method Call Message</c> as a data source.</param>
        /// <returns>
        /// A string with lite info about the <c>Method Call Message</c>.
        /// </returns>
        private static string GetLiteInfoFromCallMessage(IMethodCallMessage message)
        {
            return string.Format(CultureInfo.CurrentCulture, @"[I][IN][METHOD SIGNATURE: {0}]", message.MethodBase.ToString());
        }

        /// <summary>
        /// Gets medium info from the <c>Method Call Message</c>.
        /// </summary>
        /// <param name="message">The <c>Method Call Message</c> as a data source.</param>
        /// <returns>
        /// A string with lite info about the <c>Method Call Message</c>.
        /// </returns>
        private static string GetMediumInfoFromCallMessage(IMethodCallMessage message)
        {
            string liteInfo = GetLiteInfoFromCallMessage(message);

            string methodArgs = string.Empty;

            if (message.ArgCount > 0)
            {
                for (int i = 0; i < message.ArgCount; i++)
                {
                    methodArgs += string.Format(CultureInfo.CurrentCulture, @"{0}, ", message.Args[i].ToString());
                }

                methodArgs = methodArgs.EndsWith(@", ", StringComparison.OrdinalIgnoreCase) ? methodArgs.Substring(0, methodArgs.Length - 2) : methodArgs;
            }

            return liteInfo + string.Format(CultureInfo.CurrentCulture, @"[PARAMETERS: {0}]", string.IsNullOrEmpty(methodArgs) ? @"No parameters" : methodArgs);
        }

        /// <summary>
        /// Gets full info from the <c>Method Call Message</c>.
        /// </summary>
        /// <param name="message">The <c>Method Call Message</c> as a data source.</param>
        /// <returns>
        /// A string with lite info about the <c>Method Call Message</c>.
        /// </returns>
        private static string GetFullInfoFromCallMessage(IMethodCallMessage message)
        {
            return GetMediumInfoFromCallMessage(message);
        }

        #endregion

        #region Info from IMethodReturnMessage

        /// <summary>
        /// Gets lite info from the <c>Method Return Message</c>.
        /// </summary>
        /// <param name="message">The <c>Method Return Message</c> as a data source.</param>
        /// <returns>
        /// A string with lite info about the <c>Method Return Message</c>.
        /// </returns>
        private static string GetLiteInfoFromReturnMessage(IMethodReturnMessage message)
        {
            return message.Exception == null ?
                string.Format(CultureInfo.InvariantCulture, @"[I][OUT][METHOD SIGNATURE: {0}]", message.MethodBase.ToString()) :
                string.Format(CultureInfo.InvariantCulture, @"[E][OUT][EXCEPTION MESSAGE: {0}]", message.Exception.Message.Trim());
        }

        /// <summary>
        /// Gets medium info from the <c>Method Return Message</c>.
        /// </summary>
        /// <param name="message">The <c>Method Return Message</c> as a data source.</param>
        /// <returns>
        /// A string with medium info about the <c>Method Return Message</c>.
        /// </returns>
        private static string GetMediumInfoFromReturnMessage(IMethodReturnMessage message)
        {
            string liteInfo = GetLiteInfoFromReturnMessage(message);

            return message.Exception == null ?
                string.Format(CultureInfo.InvariantCulture, @"{0}[RETURN VALUE: {1}]", liteInfo, message.ReturnValue == null ? @"No return value" : message.ReturnValue.ToString()) :
                string.Format(CultureInfo.InvariantCulture, @"{0}[STACK TRACE: {1}]", liteInfo, message.Exception.StackTrace.Trim());
        }

        /// <summary>
        /// Gets full info from the <c>Method Return Message</c>.
        /// </summary>
        /// <param name="message">The <c>Method Return Message</c> as a data source.</param>
        /// <param name="stopwatchInfo">Info about the message time metrics (execution total time).</param>
        /// <returns>
        /// A string with full info about the <c>Method Return Message</c>.
        /// </returns>
        private static string GetFullInfoFromReturnMessage(IMethodReturnMessage message, string stopwatchInfo)
        {
            string mediumInfo = GetMediumInfoFromReturnMessage(message);

            return string.IsNullOrEmpty(stopwatchInfo) ?
                mediumInfo :
                string.Format(CultureInfo.InvariantCulture, @"{0}[STOPWATCH: {1} ms]", mediumInfo, stopwatchInfo);
        }

        #endregion
    }
}
