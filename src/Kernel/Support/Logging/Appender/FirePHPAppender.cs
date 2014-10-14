using System;
using System.Globalization;

using X3Platform.Web.Util;

using X3Platform.Logging.Layout;
using X3Platform.Logging.Core;

namespace X3Platform.Logging.Appender
{
    /// <summary>Appends logging events to the firephp.</summary>
    public class FirePHPAppender : AppenderSkeleton
    {
        /// <summary>¹¹Ôìº¯Êý</summary>
        public FirePHPAppender()
        {
        }

        /// <summary>
        /// This method is called by the <see cref="AppenderSkeleton.DoAppend(LoggingEvent)"/> method.
        /// </summary>
        /// <param name="loggingEvent">The event to log.</param>
        /// <remarks>
        /// <para>
        /// Writes the event to the console.
        /// </para>
        /// <para>
        /// The format of the output will depend on the appender's layout.
        /// </para>
        /// </remarks>
        override protected void Append(LoggingEvent loggingEvent)
        {
            string text = RenderLoggingEvent(loggingEvent);

            switch (loggingEvent.Level.Name)
            {
                case "DEBUG":
                    FirePHPHelper.Debug(text);
                    break;
                case "INFO":
                    FirePHPHelper.Info(text);
                    break;
                case "WARN":
                    FirePHPHelper.Warn(text);
                    break;
                case "ERROR":
                    FirePHPHelper.Error(text);
                    break;
                default:
                    FirePHPHelper.Log(RenderLoggingEvent(loggingEvent));
                    break;
            }
        }
    }
}
