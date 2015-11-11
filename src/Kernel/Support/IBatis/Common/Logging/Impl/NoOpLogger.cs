#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 471478 $
 * $Date: 2006-11-05 18:53:41 +0100 (dim., 05 nov. 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

using System;
using Common.Logging;

namespace X3Platform.IBatis.Common.Logging.Impl
{
	/// <summary>
	/// Silently ignores all log messages.
	/// </summary>
	public sealed class NoOpLogger: ILog
	{
		#region Members of ILog

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(object message)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Fatal(object message, Exception e)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Info(object message, Exception e)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
			// NOP - no operation
		}


		/// <summary>
		/// Ignores message.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
			// NOP - no operation
		}

		/// <summary>
		/// Always returns <see langword="false" />.
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// Always returns <see langword="false" />.
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return false; }

		}

		/// <summary>
		/// Always returns <see langword="false" />.
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// Always returns <see langword="false" />.
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return false; }
		}

		/// <summary>
		/// Always returns <see langword="false" />.
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return false; }
		}


        public bool IsTraceEnabled
        {
            get { return false; }
        }
		#endregion

        #region ILog Members
        public void Trace(object message)
        {
        }

        public void Trace(object message, Exception exception)
        {
        }
        #endregion

        #region ILog Members

        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void Debug(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void Error(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void FatalFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IVariablesContext GlobalVariablesContext
        {
            get { throw new NotImplementedException(); }
        }

        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void Info(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Info(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IVariablesContext ThreadVariablesContext
        {
            get { throw new NotImplementedException(); }
        }

        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void Trace(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Trace(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void TraceFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void TraceFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void Warn(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warn(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}