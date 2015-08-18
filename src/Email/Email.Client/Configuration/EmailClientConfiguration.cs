namespace X3Platform.Email.Client.Configuration
{
  using System.Configuration;

  using X3Platform.Configuration;
  using System.Xml;

  /// <summary>�ʼ��ͻ��˵�������Ϣ</summary>
  public class EmailClientConfiguration : X3Platform.Configuration.XmlConfiguraton
  {
    /// <summary>����Ӧ�õ�����</summary>
    public const string ApplicationName = "Email.Client";

    /// <summary>�������������</summary>
    public const string SectionName = "email.client";

    /// <summary>��ȡ������������</summary>
    public override string GetSectionName()
    {
      return SectionName;
    }

    #region ����:API ����
    private EmailSmtp m_EmailSmtp;

    /// <summary>API ��������</summary>
    public EmailSmtp EmailSmtp
    {
      get { return this.m_EmailSmtp; }
    }
    #endregion

    #region ����:Configure(XmlElement element)
    /// <summary>����XmlԪ�����ö�����Ϣ</summary>
    /// <param name="element">���ýڵ��XmlԪ��</param>
    public override void Configure(XmlElement element)
    {
      if (this.m_EmailSmtp == null)
      {
        this.m_EmailSmtp = new EmailSmtp();
      }

      // ���ؼ���:Methods
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
