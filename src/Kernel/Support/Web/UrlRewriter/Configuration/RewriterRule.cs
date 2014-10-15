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

using System;
using System.Xml.Serialization;

namespace X3Platform.Web.UrlRewriter.Configuration
{
    /// <summary>��ַ��д����</summary>
    [Serializable()]
    public class RewriterRule
    {
        #region 属性:Lookfor
        private string lookfor;

        /// <summary>������ַ</summary>
        [XmlElement("lookfor")]
        public string Lookfor
        {
            get { return lookfor; }
            set { lookfor = value; }
        }
        #endregion

        #region 属性:Sendto
        private string sendto;

        /// <summary>��д��ַ</summary>
        [XmlElement("sendto")]
        public string Sendto
        {
            get { return sendto; }
            set { sendto = value; }
        }
        #endregion

        #region 属性:Remark
        private string remark;

        /// <summary>��ע��Ϣ</summary>
        [XmlElement("remark")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion
    }
}
