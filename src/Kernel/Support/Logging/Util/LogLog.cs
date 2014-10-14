#region Copyright & License
//
// Copyright 2001-2005 The Apache Software Foundation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Configuration;
using System.Diagnostics;

namespace X3Platform.Logging.Util
{
	/// <summary>
	/// Outputs log statements from within the X3Platform.Logging assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Log4net components cannot make X3Platform.Logging logging calls. However, it is
	/// sometimes useful for the user to learn about what X3Platform.Logging is
	/// doing.
	/// </para>
	/// <para>
	/// All X3Platform.Logging internal debug calls go to the standard output stream
	/// whereas internal error messages are sent to the standard error output 
	/// stream.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class LogLog
	{
		#region Private Instance Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LogLog" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </para>
		/// </remarks>
		private LogLog()
		{
		}

		#endregion Private Instance Constructors

		#region Static Constructor

		/// <summary>
		/// Static constructor that initializes logging by reading 
		/// settings from the application configuration file.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <c>X3Platform.Logging.Internal.Debug</c> application setting
		/// controls internal debugging. This setting should be set
		/// to <c>true</c> to enable debugging.
		/// </para>
		/// <para>
		/// The <c>X3Platform.Logging.Internal.Quiet</c> application setting
		/// suppresses all internal logging including error messages. 
		/// This setting should be set to <c>true</c> to enable message
		/// suppression.
		/// </para>
		/// </remarks>
		static LogLog()
		{
#if !NETCF
			try
			{
				InternalDebugging = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("X3Platform.Logging.Internal.Debug"), false);
				QuietMode = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("X3Platform.Logging.Internal.Quiet"), false);
			}
			catch(Exception ex)
			{
				// If an exception is thrown here then it looks like the config file does not
				// parse correctly.
				//
				// We will leave debug OFF and print an Error message
				Error("LogLog: Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", ex);
			}
#endif
		}

		#endregion Static Constructor

		#region Public Static Properties

		/// <summary>
		/// Gets or sets a value indicating whether X3Platform.Logging internal logging
		/// is enabled or disabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if X3Platform.Logging internal logging is enabled, otherwise 
		/// <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// When set to <c>true</c>, internal debug level logging will be 
		/// displayed.
		/// </para>
		/// <para>
		/// This value can be set by setting the application setting 
		/// <c>X3Platform.Logging.Internal.Debug</c> in the application configuration
		/// file.
		/// </para>
		/// <para>
		/// The default value is <c>false</c>, i.e. debugging is
		/// disabled.
		/// </para>
		/// </remarks>
		/// <example>
		/// <para>
		/// The following example enables internal debugging using the 
		/// application configuration file :
		/// </para>
		/// <code lang="XML" escaped="true">
		/// <configuration>
		///		<appSettings>
		///			<add key="X3Platform.Logging.Internal.Debug" value="true" />
		///		</appSettings>
		/// </configuration>
		/// </code>
		/// </example>
		public static bool InternalDebugging
		{
			get { return s_debugEnabled; }
			set { s_debugEnabled = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether X3Platform.Logging should generate no output
		/// from internal logging, not even for errors. 
		/// </summary>
		/// <value>
		/// <c>true</c> if X3Platform.Logging should generate no output at all from internal 
		/// logging, otherwise <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// When set to <c>true</c> will cause internal logging at all levels to be 
		/// suppressed. This means that no warning or error reports will be logged. 
		/// This option overrides the <see cref="InternalDebugging"/> setting and 
		/// disables all debug also.
		/// </para>
		/// <para>This value can be set by setting the application setting
		/// <c>X3Platform.Logging.Internal.Quiet</c> in the application configuration file.
		/// </para>
		/// <para>
		/// The default value is <c>false</c>, i.e. internal logging is not
		/// disabled.
		/// </para>
		/// </remarks>
		/// <example>
		/// The following example disables internal logging using the 
		/// application configuration file :
		/// <code lang="XML" escaped="true">
		/// <configuration>
		///		<appSettings>
		///			<add key="X3Platform.Logging.Internal.Quiet" value="true" />
		///		</appSettings>
		/// </configuration>
		/// </code>
		/// </example>
		public static bool QuietMode
		{
			get { return s_quietMode; }
			set { s_quietMode = value; }
		}

		#endregion Public Static Properties

		#region Public Static Methods

		/// <summary>
		/// Test if LogLog.Debug is enabled for output.
		/// </summary>
		/// <value>
		/// <c>true</c> if Debug is enabled
		/// </value>
		/// <remarks>
		/// <para>
		/// Test if LogLog.Debug is enabled for output.
		/// </para>
		/// </remarks>
		public static bool IsDebugEnabled
		{
			get { return s_debugEnabled && !s_quietMode; }
		}

		/// <summary>
		/// Writes X3Platform.Logging internal debug messages to the 
		/// standard output stream.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <remarks>
		/// <para>
		///	All internal debug messages are prepended with 
		///	the string "X3Platform.Logging: ".
		/// </para>
		/// </remarks>
		public static void Debug(string message) 
		{
			if (IsDebugEnabled) 
			{
				EmitOutLine(PREFIX + message);
			}
		}

		/// <summary>
		/// Writes X3Platform.Logging internal debug messages to the 
		/// standard output stream.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">An exception to log.</param>
		/// <remarks>
		/// <para>
		///	All internal debug messages are prepended with 
		///	the string "X3Platform.Logging: ".
		/// </para>
		/// </remarks>
		public static void Debug(string message, Exception exception) 
		{
			if (IsDebugEnabled) 
			{
				EmitOutLine(PREFIX + message);
				if (exception != null)
				{
					EmitOutLine(exception.ToString());
				}
			}
		}
  
		/// <summary>
		/// Test if LogLog.Warn is enabled for output.
		/// </summary>
		/// <value>
		/// <c>true</c> if Warn is enabled
		/// </value>
		/// <remarks>
		/// <para>
		/// Test if LogLog.Warn is enabled for output.
		/// </para>
		/// </remarks>
		public static bool IsWarnEnabled
		{
			get { return !s_quietMode; }
		}

		/// <summary>
		/// Writes X3Platform.Logging internal warning messages to the 
		/// standard error stream.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <remarks>
		/// <para>
		///	All internal warning messages are prepended with 
		///	the string "X3Platform.Logging:WARN ".
		/// </para>
		/// </remarks>
		public static void Warn(string message) 
		{
			if (IsWarnEnabled)
			{
				EmitErrorLine(WARN_PREFIX + message);
			}
		}  

		/// <summary>
		/// Writes X3Platform.Logging internal warning messages to the 
		/// standard error stream.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">An exception to log.</param>
		/// <remarks>
		/// <para>
		///	All internal warning messages are prepended with 
		///	the string "X3Platform.Logging:WARN ".
		/// </para>
		/// </remarks>
		public static void Warn(string message, Exception exception) 
		{
			if (IsWarnEnabled)
			{
				EmitErrorLine(WARN_PREFIX + message);
				if (exception != null) 
				{
					EmitErrorLine(exception.ToString());
				}
			}
		} 

		/// <summary>
		/// Test if LogLog.Error is enabled for output.
		/// </summary>
		/// <value>
		/// <c>true</c> if Error is enabled
		/// </value>
		/// <remarks>
		/// <para>
		/// Test if LogLog.Error is enabled for output.
		/// </para>
		/// </remarks>
		public static bool IsErrorEnabled
		{
			get { return !s_quietMode; }
		}

		/// <summary>
		/// Writes X3Platform.Logging internal error messages to the 
		/// standard error stream.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <remarks>
		/// <para>
		///	All internal error messages are prepended with 
		///	the string "X3Platform.Logging:ERROR ".
		/// </para>
		/// </remarks>
		public static void Error(string message) 
		{
			if (IsErrorEnabled)
			{
				EmitErrorLine(ERR_PREFIX + message);
			}
		}  

		/// <summary>
		/// Writes X3Platform.Logging internal error messages to the 
		/// standard error stream.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="exception">An exception to log.</param>
		/// <remarks>
		/// <para>
		///	All internal debug messages are prepended with 
		///	the string "X3Platform.Logging:ERROR ".
		/// </para>
		/// </remarks>
		public static void Error(string message, Exception exception) 
		{
			if (IsErrorEnabled)
			{
				EmitErrorLine(ERR_PREFIX + message);
				if (exception != null) 
				{
					EmitErrorLine(exception.ToString());
				}
			}
		}  

		#endregion Public Static Methods

		/// <summary>
		/// Writes output to the standard output stream.  
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <remarks>
		/// <para>
		/// Writes to both Console.Out and System.Diagnostics.Trace.
		/// Note that the System.Diagnostics.Trace is not supported
		/// on the Compact Framework.
		/// </para>
		/// <para>
		/// If the AppDomain is not configured with a config file then
		/// the call to System.Diagnostics.Trace may fail. This is only
		/// an issue if you are programmatically creating your own AppDomains.
		/// </para>
		/// </remarks>
		private static void EmitOutLine(string message)
		{
			try
			{
#if NETCF
				Console.WriteLine(message);
				//System.Diagnostics.Debug.WriteLine(message);
#else
				Console.Out.WriteLine(message);
				Trace.WriteLine(message);
#endif
			}
			catch
			{
				// Ignore exception, what else can we do? Not really a good idea to propagate back to the caller
			}
		}

		/// <summary>
		/// Writes output to the standard error stream.  
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <remarks>
		/// <para>
		/// Writes to both Console.Error and System.Diagnostics.Trace.
		/// Note that the System.Diagnostics.Trace is not supported
		/// on the Compact Framework.
		/// </para>
		/// <para>
		/// If the AppDomain is not configured with a config file then
		/// the call to System.Diagnostics.Trace may fail. This is only
		/// an issue if you are programmatically creating your own AppDomains.
		/// </para>
		/// </remarks>
		private static void EmitErrorLine(string message)
		{
			try
			{
#if NETCF
				Console.WriteLine(message);
				//System.Diagnostics.Debug.WriteLine(message);
#else
				Console.Error.WriteLine(message);
				Trace.WriteLine(message);
#endif
			}
			catch
			{
				// Ignore exception, what else can we do? Not really a good idea to propagate back to the caller
			}
		}

		#region Private Static Fields

		/// <summary>
		///  Default debug level
		/// </summary>
		private static bool s_debugEnabled = false;

		/// <summary>
		/// In quietMode not even errors generate any output.
		/// </summary>
		private static bool s_quietMode = false;

		private const string PREFIX			= "X3Platform.Logging: ";
		private const string ERR_PREFIX		= "X3Platform.Logging:ERROR ";
		private const string WARN_PREFIX	= "X3Platform.Logging:WARN ";

		#endregion Private Static Fields
	}
}
