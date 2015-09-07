namespace X3Platform.Globalization
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Text;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Xml;
  using Common.Logging;
  using X3Platform.Configuration;
  using X3Platform.Util;

  /// <summary>本地化脚本文件</summary>
  public class I18nScript
  {
    private ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.ToString());

    #region 属性:Instance
    private static volatile I18nScript instance = null;

    private static object lockObject = new object();

    /// <summary>实例</summary>
    public static I18nScript Instance
    {
      get
      {
        if (instance == null)
        {
          lock (lockObject)
          {
            if (instance == null)
            {
              instance = new I18nScript();
            }
          }
        }

        return instance;
      }
    }
    #endregion

    private I18nScript() { }

    private string[] ignoreFiles = new string[] { "menu.xml" };

    public void Init()
    {
      // 默认目录
      string defaultDirectory = KernelConfigurationView.Instance.ApplicationPathRoot + "locales/" + KernelConfigurationView.Instance.CultureName;
      // 本地化设置目录
      string[] directories = Directory.GetDirectories(KernelConfigurationView.Instance.ApplicationPathRoot + "locales");

      XmlDocument doc = new XmlDocument();

      XmlDocument i18nDoc = new XmlDocument();

      foreach (var directory in directories)
      {
        Console.WriteLine(directory);

        StringBuilder outString = new StringBuilder();

        string[] files = Directory.GetFiles(defaultDirectory, "*.xml");

        outString.Append(@"(function(i18n){ var init = function(destination, source) { for(var property in source) { destination[property] = source[property]; } return destination; }; i18n = init(i18n, {");

        foreach (var file in files)
        {
          // 过滤忽略的文件
          if (ignoreFiles.Contains(Path.GetFileName(file))) { continue; }

          outString.Append(Path.GetFileNameWithoutExtension(file) + ":");

          outString.Append("{");

          doc.Load(file);

          i18nDoc.Load(Path.Combine(directory, Path.GetFileName(file)));

          XmlNodeList nodes = doc.DocumentElement.ChildNodes;

          foreach (XmlNode node in nodes)
          {
            if (node.NodeType == XmlNodeType.Comment) { continue; }

            if (node.ChildNodes.Count > 1)
            {
              XmlNodeList childNodes = node.ChildNodes;

              outString.Append("\"" + node.Attributes["name"].Value + "\":{");

              foreach (XmlNode childNode in childNodes)
              {
                outString.Append("\"" + childNode.Attributes["name"].Value + "\":\"" + GetInnerText(doc, i18nDoc, string.Concat("resources/", node.Name, "[@name='" + node.Attributes["name"].Value + "']/", childNode.Name, "[@name='" + childNode.Attributes["name"].Value + "']")) + "\",");
              }

              StringHelper.TrimEnd(outString, ",");

              outString.Append("},");
            }
            else
            {
              outString.Append("\"" + node.Attributes["name"].Value + "\":\"" + GetInnerText(doc, i18nDoc, string.Concat("resources/", node.Name, "[@name='" + node.Attributes["name"].Value + "']")) + "\",");
            }
          }

          StringHelper.TrimEnd(outString, ",");

          outString.Append("},");
        }

        StringHelper.TrimEnd(outString, ",");

        outString.AppendLine(@"}); window.i18n = i18n; return i18n; })(typeof i18n !== 'undefined' ? i18n : {});");

        try
        {
          File.WriteAllText(directory + "/i18n.js", outString.ToString(), Encoding.UTF8);
        }
        catch (Exception ex)
        {
          logger.Error("Failed to write to " + directory + "/i18n.js");

          logger.Error(ex.Message);
        }
      }
    }

    /// <summary>
    /// 获取文本信息
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="i18nDoc"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    private string GetInnerText(XmlDocument doc, XmlDocument i18nDoc, string xpath)
    {
      XmlNode node = i18nDoc.SelectSingleNode(xpath);

      if (node == null)
      {
        node = doc.SelectSingleNode(xpath);
      }

      return node.InnerText;
    }

    public string GetFile()
    {
      string defaultFile = KernelConfigurationView.Instance.ApplicationPathRoot + "locales/" + KernelConfigurationView.Instance.CultureName + "/i18n.js";
      string i18nFile = KernelConfigurationView.Instance.ApplicationPathRoot + "locales/" + Thread.CurrentThread.CurrentCulture.Name + "/i18n.js";
      
      if (File.Exists(i18nFile))
      {
        return "/locales/" + Thread.CurrentThread.CurrentCulture.Name + "/i18n.js";
      }
      else
      {
        return "/locales/" + KernelConfigurationView.Instance.CultureName + "/i18n.js";
      }
    }
  }
}
