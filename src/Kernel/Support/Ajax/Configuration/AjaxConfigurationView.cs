// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System.IO;

using X3Platform.Configuration;
using X3Platform.Yaml.RepresentationModel;

namespace X3Platform.Ajax.Configuration
{
    /// <summary>Ajax������ͼ</summary>
    public class AjaxConfigurationView : XmlConfigurationView<AjaxConfiguration>
    {
        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Ajax.config";

        /// <summary>������Ϣ��ȫ��ǰ׺</summary>
        private const string configGlobalPrefix = AjaxConfiguration.ApplicationName;

        #region ��̬属性:Instance
        private static volatile AjaxConfigurationView instance = null;

        private static object lockObject = new object();

        /// <summary>ʵ��</summary>
        public static AjaxConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AjaxConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:AjaxConfigurationView()
        /// <summary>���캯��</summary>
        private AjaxConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion

        #region 属性:Reload()
        /// <summary>���¼���������Ϣ</summary>
        public override void Reload()
        {
            base.Reload();

            // ��������Ϣ���ص�ȫ�ֵ�������
            KernelConfigurationView.Instance.AddKeyValues(configGlobalPrefix, this.Configuration.Keys, false);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ�������
        // -------------------------------------------------------

        #region 属性:CamelStyle
        private string m_CamelStyle = string.Empty;

        /// <summary>CamelStyle Camel��д��ʽ, On : �� | Off : ��</summary>
        public string CamelStyle
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_CamelStyle))
                {
                    // ��ȡ����ȫ����Ϣ
                    this.m_CamelStyle = KernelConfigurationView.Instance.GetKeyValue(configGlobalPrefix, "CamelStyle", this.Configuration.Keys);

                    // ���������ļ���û�����ã�����һ��Ĭ��ֵ��
                    if (string.IsNullOrEmpty(this.m_CamelStyle))
                    {
                        this.m_CamelStyle = "Off";
                    }

                    this.m_CamelStyle = this.m_CamelStyle.ToUpper();
                }

                return this.m_CamelStyle;
            }
        }
        #endregion
    }
}
