namespace X3Platform.Web.Tests.Customize.Widgets
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Web;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>Web 部件测试</summary>
    [TestClass]
    public class WidgetTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: 在此	添加测试逻辑
            //

            //通过sogou查询IP地址

            string result = string.Empty;

            string url = string.Format("http://www.sogou.com/web?query={0}", "123.112.11.60");

            byte[] bytes = new System.Net.WebClient().DownloadData(url);

            string html = System.Text.Encoding.Default.GetString(bytes);

            int a = html.IndexOf("<p class=\"ff\">");

            if (a > 0)
            {
                a += 14;

                int b = html.IndexOf("</p>", a);

                if (b > a)
                {
                    result = html.Substring(a, b - a);
                }
            }

            // 3.进一步细节处理

            if (!string.IsNullOrEmpty(result) && result.IndexOf("地理位置：") > 0)
            {
                result = result.Substring(result.IndexOf("地理位置：") + 5);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            // 第二种：无标题页四川 南充 - 中国联通

            string result = string.Empty;

            string url = string.Format("http://www.google.cn/search?hl=zh-CN&q={0}", "13883729215");

            byte[] bytes = new System.Net.WebClient().DownloadData(url);

            string html = System.Text.Encoding.Default.GetString(bytes);

            int a = html.IndexOf("归属地查询</a></h3>&nbsp;");

            if (a > 0)
            {
                a += 20;
                int b = html.IndexOf("<div style=\"margin:2px 0\">", a);
                if (b > a)
                {
                    result = html.Substring(a, b - a);
                }
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            // 第二种：获取最近三天天气情况

            string result = string.Empty;

            //获取最近三天天气情况
            string url = string.Format("http://www.google.cn/search?hl=zh-CN&q={0}", HttpUtility.UrlEncode("天气 上海 ", System.Text.Encoding.UTF8).ToUpper());

            byte[] bytes = new System.Net.WebClient().DownloadData(url);

            string html = System.Text.Encoding.Default.GetString(bytes);

            int a = html.IndexOf("<div class=e>");

            if (a > 0)
            {
                a += 13;

                int b = html.IndexOf("<h2 class=hd>", a);

                if (b > a)
                {
                    result = html.Substring(a - 13, b - a + 13).Replace("src=\"/", "src=\"http://www.google.cn/");
                }
            }

            // 添加到 iGoogle</a></div>

            // 3.进一步细节处理

            if (!string.IsNullOrEmpty(result) && result.IndexOf("添加到 iGoogle</a></div>") > 0)
            {
                result = "<table>" + result.Substring(result.IndexOf("添加到 iGoogle</a></div>") + 21);
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            // 第二种：获取最近三天天气情况
            string result = string.Empty;

            string url = string.Format("http://www.google.cn/search?hl=zh-CN&q={0}", HttpUtility.UrlEncode("农历 1981-6-5", System.Text.Encoding.UTF8).ToUpper());

            // 1.获取数据

            byte[] bytes = new System.Net.WebClient().DownloadData(url);

            string html = System.Text.Encoding.Default.GetString(bytes);

            // 2. 截取内容数据

            int a = html.IndexOf("<div class=e>");

            if (a > 0)
            {
                a += 13;

                int b = html.IndexOf("</table></table></div></div>", a);

                if (b > a)
                {
                    result = html.Substring(a - 13, b - a + 29).Replace("src=\"/", "src=\"http://www.google.cn/");
                }
            }

            // 3.进一步细节处理

            if (!string.IsNullOrEmpty(result) && result.IndexOf("<div class=std>") > 0)
            {
                result = result.Substring(result.IndexOf("<div class=std>") + 15);
            }
        }
    }
}
