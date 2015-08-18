namespace X3Platform.Email.Client.Configuration
{
  using System.Configuration;

  using X3Platform.Configuration;
  using System.Xml;

  /// <summary>邮件客户端的配置信息</summary>
  public class EmailClientConfiguration : X3Platform.Configuration.XmlConfiguraton
  {
    /// <summary>所属应用的名称</summary>
    public const string ApplicationName = "Email.Client";

    /// <summary>配置区域的名称</summary>
    public const string SectionName = "email.client";

    /// <summary>获取配置区的名称</summary>
    public override string GetSectionName()
    {
      return SectionName;
    }

    #region 属性:API 方法
    private EmailSmtp m_EmailSmtp;

    /// <summary>API 方法集合</summary>
    public EmailSmtp EmailSmtp
    {
      get { return this.m_EmailSmtp; }
    }
    #endregion

    #region 函数:Configure(XmlElement element)
    /// <summary>根据Xml元素配置对象信息</summary>
    /// <param name="element">配置节点的Xml元素</param>
    public override void Configure(XmlElement element)
    {
      if (this.m_EmailSmtp == null)
      {
        this.m_EmailSmtp = new EmailSmtp();
      }

      // 加载集合:Methods
      XmlNode node = element.SelectSingleNode(@"smtp");

      if (node != null)
      {
        foreach (XmlNode childNode in node.ChildNodes)
        {
          if (childNode.NodeType == XmlNodeType.Element)
          {
            XmlElement childElement = (XmlElement)childNode;

            XmlConfiguratonOperator.SetParameter(this.EmailSmtp, (XmlElement)childElement);
          }
        }
      }
    }
    #endregion
  }
}
