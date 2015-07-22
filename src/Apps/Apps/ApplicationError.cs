namespace X3Platform.Apps
{
  #region Using Libraries
  using System;
  using System.Web;
  using System.Text;

  using X3Platform.Apps.Model;
  #endregion

  /// <summary>应用问题管理</summary>
  public sealed class ApplicationError
  {
    /// <summary>输出问题信息并终止HTTP响应</summary>
    public static void Write(int code)
    {
      Write(code.ToString());
    }

    /// <summary>输出问题信息并终止HTTP响应</summary>
    /// <param name="code"></param>
    public static void Write(string code)
    {
      ApplicationErrorInfo applicationErrorInfo = AppsContext.Instance.ApplicationErrorService[code];

      StringBuilder outString = new StringBuilder();

      outString.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");

      outString.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");

      outString.AppendLine("<head>");
      outString.AppendLine("<title>【" + applicationErrorInfo.Code + "】" + applicationErrorInfo.Title + "</title>");
      outString.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
      outString.AppendLine("<style type=\"text/css\" >");
      outString.AppendLine("html, body { font-family: '微软雅黑'; background-color:#fff; margin:0px;}");
      outString.AppendLine(".window-exception-wrapper {background-image: url(/resources/images/exception/header_bg.png); background-repeat: repeat-x; line-height:200%; font-size:12px; height: 100%; padding:30px 120px 4px 140px; }");
      outString.AppendLine(".window-exception-wrapper a { color: #007ab7; }");
      outString.AppendLine(".window-exception-logo {position: absolute; left: 0; top: 0;}");
      outString.AppendLine(".window-exception-header { border-bottom:none; color:#000; border-bottom: 1px solid #ccc; margin-bottom: 10px; padding: 0 0 6px 0; }");
      outString.AppendLine(".window-exception-container { padding: 0 0 0 10px; line-height: 200%; }");
      outString.AppendLine(".window-exception-container div.text { font-weight: bold; padding:0 0 8px 0; }");
      outString.AppendLine(".window-exception-container div.description { padding:8px; border:1px solid #ccc; border-bottom: 3px solid #ccc; line-height: 150%; }");
      outString.AppendLine(".window-exception-container div.description ul { margin:0; padding: 0 0 0 20px; }");
      outString.AppendLine(".window-exception-container div.description li { padding: 6px 0 0px 0px; }");
      outString.AppendLine(".window-exception-container div.tooltip { color:#666; text-align:right; padding:4px 0 8px 0; }");
      outString.AppendLine("</style>");

      outString.AppendLine("<head>");

      outString.AppendLine("<body>");
      outString.AppendLine("<div class=\"window-exception-wrapper\">");
      outString.AppendLine("<div class=\"window-exception-logo\"><img alt=\"怪兽图标\" src=\"/resources/images/exception/header_logo.png\" /></div>");
      // 问题标题
      outString.AppendLine("<div class=\"window-exception-header\"><h1>" + applicationErrorInfo.Title + "</h1></div>");
      // 问题描述
      outString.AppendLine("<div class=\"window-exception-container\" >");
      // 当前浏览器信息
      outString.AppendLine("<div>当前浏览器: <script>document.write(navigator.userAgent);</script></div>");
      // 当前地址
      outString.AppendLine("<div>当前地址: <script>document.write(location.href);</script></div>");
      // 当前用户
      outString.AppendLine("<div>当前用户: <script>var value = ''; var search = 'memberToken='; if (document.cookie.length > 0) { var offset = document.cookie.indexOf(search); if (offset != -1) { offset += search.length; var end = document.cookie.indexOf(';', offset); if (end == -1) end = document.cookie.length; value = unescape(document.cookie.substring(offset, end)); document.write(eval('(' + value + ')').ajaxStorage.loginName); } }</script></div>");
      // HTTP Status Code
      outString.AppendLine("<div>HTTP Status Code: " + applicationErrorInfo.StatusCode + "</div>");
      // 问题发生时间
      outString.AppendLine("<div>问题发生时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</div>");
      // 问题可能的原因是
      outString.AppendLine("<div class=\"text\" >问题可能的原因是:</div>");
      outString.AppendLine("<div class=\"description\" >" + applicationErrorInfo.Description + "</div>");
      outString.AppendLine("<div class=\"tooltip\">可以把以上信息发送给系统管理员，帮助我们解决问题。</div>");
      outString.AppendLine("</div>");

      outString.AppendLine("</div>");
      outString.AppendLine("</div>");
      outString.AppendLine("</body>");
      outString.AppendLine("</html>");

      HttpContext.Current.Response.StatusCode = applicationErrorInfo.StatusCode;
      HttpContext.Current.Response.Write(outString.ToString());
      HttpContext.Current.Response.End();
    }
  }
}
