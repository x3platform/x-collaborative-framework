namespace X3Platform.AttachmentStorage.Mvc.Controllers
{
  using System;
  using System.Web.Mvc;

  using X3Platform.Json;
  using X3Platform.Membership;
  using X3Platform.DigitalNumber;
  using X3Platform.Configuration;
  using X3Platform.Web.Mvc.Controllers;
  using X3Platform.Web.Mvc.Attributes;
  using X3Platform.Apps;
  using X3Platform.Apps.Model;

  using X3Platform.AttachmentStorage;
  using X3Platform.AttachmentStorage.Configuration;
  using System.Web;
  using System.Text;
  using X3Platform.Util;
  using System.IO;

  [LoginFilter]
  public class HomeController : CustomController
  {
    #region 函数:Index()
    /// <summary>主页</summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      // 所属应用信息
      ApplicationInfo application = ViewBag.application = AppsContext.Instance.ApplicationService[AttachmentStorageConfiguration.ApplicationName];

      return View("/views/main/attachment/default.cshtml");
    }
    #endregion

    #region 函数:Download(string options)
    /// <summary>下载</summary>
    /// <param name="doc">Xml 文档对象</param>
    /// <returns>返回操作结果</returns> 
    public ActionResult Download(string options)
    {
      JsonData request = JsonMapper.ToObject(options == null ? "{}" : options);

      // 实体数据标识
      string id = !request.Keys.Contains("id") ? string.Empty : request["id"].ToString();
     
      IAttachmentFileInfo param = AttachmentStorageContext.Instance.AttachmentFileService[id];

      if (param == null)
      {
        ApplicationError.Write(404);
      }
      else
      {
        if (param != null && param.FileData == null)
        {
          // 下载分布式文件数据
          param.FileData = DistributedFileStorage.Download(param);
        }

        // [容错] 由于人为原因将服务器上的文件删除。
        if (param.FileData == null)
        {
          ApplicationError.Write(404);
        }

        Response.ContentType = "application/octet-stream";

        Response.AddHeader("Content-Disposition", "attachment;filename=" + EncodeFileName(param.AttachmentName));
        Response.BinaryWrite(param.FileData);
      }

      Response.End();

      return Content(string.Empty);
    }

    /// <summary></summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string EncodeFileName(string fileName)
    {
      string agent = Request.Headers["USER-AGENT"];

      if (!string.IsNullOrEmpty(agent) && agent.IndexOf("MSIE 6.0") != -1)
      {
        string prefix = Path.GetFileNameWithoutExtension(fileName);

        string extension = Path.GetExtension(fileName);

        //
        // 在IE6访问此页面是, 输出文件名不能长于17个中文字符.
        // Bug of IE (http://support.microsoft.com/?kbid=816868)  
        //

        //UTF8 URL encoding only works in IE  
        fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);

        // Encoded fileName cannot be more than 150  
        int limit = 150 - extension.Length;

        if (fileName.Length > limit)
        {
          // because the UTF-8 encoding scheme uses 9 bytes to represent a single CJK character  
          fileName = HttpUtility.UrlEncode(prefix.Substring(0, Math.Min(prefix.Length, limit / 9)), Encoding.UTF8);

          fileName = fileName + extension;
        }

        return fileName;
      }
      else if (!string.IsNullOrEmpty(agent) && agent.IndexOf("MSIE") != -1)
      {
        return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
      }
      else if (!string.IsNullOrEmpty(agent) && agent.IndexOf("Mozilla") != -1)
      {
        // return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
        return "=?UTF-8?B?" + StringHelper.ToBase64(fileName, "UTF-8") + "?=";
      }
      else
      {
        return HttpUtility.UrlEncode(fileName, Encoding.UTF8);
      }
    }

    #endregion
  }
}
