using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Web;

namespace X3Platform.Web.Util
{
    public static class FirePHPHelper
    {
        public class FirePHPLog
        {
            public string logType;
            public object header; // anonymous type
            public object msg;
        }

        private static Dictionary<string, string> _BaseHeaders;

        private static bool IsEnabled = true;
        private static int logCounter = 0;

        private static string headerInitSlotName = "FirePHP.HeaderInit";

        static FirePHPHelper()
        {
            _BaseHeaders = new Dictionary<string, string>();

            _BaseHeaders.Add("X-Wf-Protocol-1", "http://meta.wildfirehq.org/Protocol/JsonStream/0.2");
            _BaseHeaders.Add("X-Wf-1-Plugin-1", "http://meta.firephp.org/Wildfire/Plugin/FirePHP/Library-FirePHPCore/0.2.0");
            _BaseHeaders.Add("X-Wf-1-Structure-1", "http://meta.firephp.org/Wildfire/Structure/FirePHP/FirebugConsole/0.1");
        }

        private static bool HeadersInit
        {
            get
            {
                if (HttpContext.Current.Items.Contains(headerInitSlotName))
                {
                    return (bool)HttpContext.Current.Items[headerInitSlotName];
                }
                else
                {
                    return false;
                }
            }

            set
            {
                if (HttpContext.Current.Items.Contains(headerInitSlotName))
                {
                    HttpContext.Current.Items[headerInitSlotName] = value;
                }
                else
                {
                    HttpContext.Current.Items.Add(headerInitSlotName, value);
                }
            }
        }

        public static void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        public static void Debug(string msg)
        {
            WriteLog("DEBUG", msg);
        }

        public static void Log(string msg)
        {
            WriteLog("LOG", msg);
        }

        public static void Info(string msg)
        {
            WriteLog("INFO", msg);
        }

        public static void Warn(string msg)
        {
            WriteLog("WARN", msg);
        }

        public static void Error(string msg)
        {
            WriteLog("ERROR", msg);
        }

        private static void WriteLog(string logType, string msg)
        {
            StackFrame callStack = new StackFrame(2, true);
            FirePHPLog log = new FirePHPLog();

            log.logType = logType;
            // log.header = new { Type = logType, File = callStack.GetFileName(), Line = callStack.GetFileLineNumber() };
            log.header = new { Type = logType, File = "", Line = "" };
            log.msg = msg;

            DumpLog(log);
        }

        public static void Exception(Exception exception)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            StackFrame callStack = new StackFrame(1, true);

            StackTrace stackTrace;
            FirePHPLog log = new FirePHPLog();

            log.logType = "EXCEPTION";
            log.header = new { Type = "EXCEPTION", File = exception.Source, Line = 1 };

            int exceptionCount = 0;
            Exception currentException = exception;

            var traceList = new List<object>();

            while (currentException.InnerException != null)
            {
                stackTrace = new StackTrace(currentException, true);
                currentException = exception.InnerException;
                exceptionCount++;

                var trace = new { file = currentException.Source, line = currentException.Source, function = currentException.Message, args = new string[0] };
                traceList.Add(trace);
            }

            if (exceptionCount > 0)
            {
                var trace = new object[exceptionCount];
            }

            stackTrace = new StackTrace(exception, true);

            log.msg = new
            {
                Class = "Exception",
                Message = exception.Message,
                File = stackTrace.GetFrame(0).GetFileName(),
                Line = stackTrace.GetFrame(0).GetFileLineNumber(),
                Type = "throw",
                Trace = traceList.ToArray()
            };

            DumpLog(log);
        }

        public static void Table(string label, string[][] table)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            StackFrame callStack = new StackFrame(1, true);

            FirePHPLog log = new FirePHPLog();

            log.logType = "TABLE";
            //log.header = new { Type = "TABLE", Label = label, File = callStack.GetFileName(), Line = callStack.GetFileLineNumber() };
            log.header = new { Type = "TABLE", Label = label, File = "", Line = "" };
            log.msg = table;

            DumpLog(log);
        }

        public static void DumpLog(FirePHPLog log)
        {
            if (!IsEnabled)
            {
                return;
            }
            HttpContext context = HttpContext.Current;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            InitHeader(context.Response);

            string json = String.Format("[{0}, {1}]", serializer.Serialize(log.header), serializer.Serialize(log.msg));

            context.Response.AppendHeader(String.Format("X-Wf-1-1-1-{0}", (logCounter + 1)), String.Format("{0}|{1}|", json.Length, json));
            if (logCounter++ > 9999)
            {
                logCounter = 0;
            }
        }

        private static Dictionary<string, string> BaseHeaders()
        {
            return _BaseHeaders;
        }

        /// <summary>Inits the header</summary>
        /// <param name="response">The response.</param>
        private static void InitHeader(HttpResponse response)
        {
            if (HeadersInit) { return; }

            foreach (KeyValuePair<string, string> keypair in BaseHeaders())
            {
                response.AppendHeader(keypair.Key, keypair.Value);
            }

            HeadersInit = true;
        }
    }
}