using System;
using System.Net;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using Microsoft.CSharp;

namespace X3Platform.Web.Util
{
    /* 调用方式  
     *   string url = "http://www.webservicex.net/globalweather.asmx" ;  
     *   string[] args = new string[2] ;  
     *   args[0] = "Hangzhou";  
     *   args[1] = "China" ;  
     *   object result = WebServiceHelper.InvokeWebService(url ,"GetWeather" ,args) ;  
     *   Response.Write(result.ToString());  
     */
    public class WebServiceHelper
    {
        /// <summary>调用服务方法</summary>
        /// <param name="url">WSDL服务地址</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object Invoke(string url, string methodName, object[] args)
        {
            return WebServiceHelper.Invoke(url, methodName, args, true);
        }

        public static object Invoke(string url, string methodName, object[] args, bool generateInMemory)
        {
            return WebServiceHelper.Invoke(url, null, methodName, args, null, generateInMemory);
        }

        public static object Invoke(string url, string className, string methodName, object[] args, string protocolName, bool generateInMemory)
        {
            //设定编译参数  
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = generateInMemory;

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.XML.dll");
            parameters.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");

            return WebServiceHelper.Invoke(url, className, methodName, args, null, parameters);
        }

        /// <summary>动态调用web服务</summary>
        /// <param name="url">WSDL服务地址</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object Invoke(string url, string className, string methodName, object[] args, string protocolName, CompilerParameters parameters)
        {
            // https://msdn.microsoft.com/zh-cn/library/system.web.services.description.servicedescriptionimporter(v=vs.110).aspx

            string namespaceRoot = "X3Platform.Web.Services.DynamicClient";

            if (string.IsNullOrEmpty(className))
            {
                className = WebServiceHelper.GetDefaultWSClassName(url);
            }

            try
            {
                // 获取 WSDL 文件
                if (url.LastIndexOf("?wsdl") != url.Length - 5 && (url.LastIndexOf("?singleWsdl") != url.Length - 11))
                {
                    url = url + "?wsdl";
                }

                WebClient client = new WebClient();

                Stream stream = client.OpenRead(url);

                ServiceDescription description = ServiceDescription.Read(stream);

                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();

                // Generate a proxy client.
                importer.Style = ServiceDescriptionImportStyle.Client;
                // Generate properties to represent primitive values.
                importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
                
                // "Soap Soap12 HttpPost HttpGet HttpSoap
                importer.ProtocolName = string.IsNullOrEmpty(protocolName) ? "Soap" : protocolName;  // Use SOAP 1.2.
                //
                importer.AddServiceDescription(description, null, null);

                CodeNamespace nmspace = new CodeNamespace(namespaceRoot);

                //生成客户端代理类代码  
                CodeCompileUnit unit = new CodeCompileUnit();

                unit.Namespaces.Add(nmspace);

                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);

                if (warning != 0)
                {
                    // Print an error message.
                    Console.WriteLine(warning);
                }

                // Generate and print the proxy code in C#.
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                // CSharpCodeProvider provider = new CSharpCodeProvider();

                if (!parameters.GenerateInMemory)
                {
                    // 可以指定你所需的任何文件名。
                    parameters.OutputAssembly = namespaceRoot + "." + className + ".dll";
                }

                //编译代理类  
                CompilerResults results = provider.CompileAssemblyFromDom(parameters, unit);

                if (true == results.Errors.HasErrors)
                {

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                    {
                        sb.Append(error.ToString());

                        sb.Append(System.Environment.NewLine);
                    }

                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                System.Reflection.Assembly assembly = results.CompiledAssembly;

                Type type = assembly.GetType(namespaceRoot + "." + className, true, true);

                object obj = Activator.CreateInstance(type);

                System.Reflection.MethodInfo method = type.GetMethod(methodName);

                return method.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        private static string GetDefaultWSClassName(string url)
        {
            string[] parts = url.Split('/');

            string[] list = parts[parts.Length - 1].Split('.');

            return list[0];
        }
    }
}