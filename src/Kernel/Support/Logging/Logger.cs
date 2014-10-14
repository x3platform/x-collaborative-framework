using System;

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
    public class Logger : Common.Logging.ILog
    {
        #region Fields

        private ILogger logger = null;

        private readonly static Type declaringType = typeof(Logger);

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
        public bool IsInfoEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Info); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Warn); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Error); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Fatal); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Debug); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTraceEnabled
        {
            get { return this.logger.IsEnabledFor(Level.Trace); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Info(object message, Exception e)
        {
            this.logger.Log(declaringType, Level.Info, message, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            this.logger.Log(declaringType, Level.Info, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Debug(object message, Exception e)
        {
            this.logger.Log(declaringType, Level.Debug, message, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            this.logger.Log(declaringType, Level.Debug, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Warn(object message, Exception e)
        {
            this.logger.Log(declaringType, Level.Warn, message, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            this.logger.Log(declaringType, Level.Warn, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Trace(object message, Exception e)
        {
            this.logger.Log(declaringType, Level.Trace, message, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Trace(object message)
        {
            this.logger.Log(declaringType, Level.Trace, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Fatal(object message, Exception e)
        {
            this.logger.Log(declaringType, Level.Fatal, message, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(object message)
        {
            this.logger.Log(declaringType, Level.Fatal, message, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Error(object message, Exception e)
        {
            this.logger.Log(declaringType, Level.Error, message, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            this.logger.Log(declaringType, Level.Error, message, null);
        }

        #endregion
    }
}