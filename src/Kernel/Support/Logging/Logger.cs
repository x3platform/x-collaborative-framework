using System;
using System.Diagnostics;

using Common.Logging;
using Common.Logging.Factory;

using X3Platform.Logging.Core;

namespace X3Platform.Logging
{
    /// <remarks>
    /// Log4net is capable of outputting extended debug information about where the current 
    /// message was generated: class name, method name, file, line, etc. Log4net assumes that the location
    /// information should be gathered relative to where Debug() was called. In IBatisNet, 
    /// Debug() is called in IBatisNet.Common.Logging.Impl.Log4NetLogger. This means that
    /// the location information will indicate that IBatisNet.Common.Logging.Impl.Log4NetLogger always made
    /// the call to Debug(). We need to know where IBatisNet.Common.Logging.ILog.Debug()
    /// was called. To do this we need to use the X3Platform.Logging.ILog.Logger.Log method and pass in a Type telling
    /// X3Platform.Logging where in the stack to begin looking for location information.
    /// </remarks>
    public class Logger : AbstractLogger
    {
        #region Fields

        private ILogger logger = null;

        private static Type callerStackBoundaryType = null;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log"></param>
        internal Logger(IInternalLog log)
        {
            this.logger = log.Logger;
        }

        #region ILog Members

        /// <summary>
        /// 
        /// </summary>
        public override bool IsInfoEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Info); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsWarnEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Warn); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsErrorEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Error); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsFatalEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Fatal); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsDebugEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Debug); }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsTraceEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Trace); }
        }

        /// <summary>
        /// Actually sends the message to the underlying log system.
        /// </summary>
        /// <param name="logLevel">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        protected override void WriteInternal(LogLevel logLevel, object message, Exception exception)
        {
            // determine correct caller - this might change due to jit optimizations with method inlining
            if (callerStackBoundaryType == null)
            {
                lock (this.GetType())
                {
                    StackTrace stack = new StackTrace();
                    Type thisType = this.GetType();

                    callerStackBoundaryType = typeof(AbstractLogger);

                    for (int i = 1; i < stack.FrameCount; i++)
                    {
                        if (!IsInTypeHierarchy(thisType, stack.GetFrame(i).GetMethod().DeclaringType))
                        {
                            callerStackBoundaryType = stack.GetFrame(i - 1).GetMethod().DeclaringType;
                            break;
                        }
                    }
                }
            }

            Level level = GetLevel(logLevel);

            logger.Log(callerStackBoundaryType, level, message, exception);
        }

        private bool IsInTypeHierarchy(Type currentType, Type checkType)
        {
            while (currentType != null && currentType != typeof(object))
            {
                if (currentType == checkType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Maps <see cref="logLevel"/> to log4net's <see cref="Level"/>
        /// </summary>
        /// <param name="logLevel"></param>
        public static Level GetLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return Level.All;
                case LogLevel.Trace:
                    return Level.Trace;
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, "unknown log level");
            }
        }

        #endregion

        /// <summary>
        /// Returns the global context for variables
        /// </summary>
        public override IVariablesContext GlobalVariablesContext
        {
            get { return new GlobalVariablesContext(); }
        }

        /// <summary>
        /// Returns the thread-specific context for variables
        /// </summary>
        public override IVariablesContext ThreadVariablesContext
        {
            get { return new ThreadVariablesContext(); }
        }
    }
}