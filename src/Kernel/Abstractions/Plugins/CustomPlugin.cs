#region Copyright & Author
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
#endregion

namespace X3Platform.Plugins
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>自定义插件</summary>
    public abstract class CustomPlugin : ICustomPlugin
    {
        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>标识</summary>
        public virtual string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary>����</summary>
        public virtual string Name
        {
            get { return m_Name; }
        }
        #endregion

        #region 属性:Version
        private string m_Version = "1.0.0.0";

        /// <summary>�汾</summary>
        public string Version
        {
            get { return m_Version; }
        }
        #endregion

        #region 属性:Author
        private string m_Author = "ruanyu83@gmail.com";

        /// <summary>����</summary>
        public virtual string Author
        {
            get { return m_Author; }
        }
        #endregion

        #region 属性:Copyright
        private string m_Copyright = "MIT";

        /// <summary>��Ȩ</summary>
        public virtual string Copyright
        {
            get { return m_Copyright; }
        }
        #endregion

        #region 属性:Url
        private string m_Url = string.Empty;

        /// <summary>������ȡ��ַ</summary>
        public virtual string Url
        {
            get { return m_Url; }
        }
        #endregion

        #region 属性:ThumbnailUrl
        private string m_ThumbnailUrl = string.Empty;

        /// <summary>����ͼ</summary>
        public virtual string ThumbnailUrl
        {
            get { return m_ThumbnailUrl; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>������Ϣ</summary>
        public virtual string Description
        {
            get { return m_Description; }
        }
        #endregion

        #region 属性:Status
        private int m_Status = 0;

        /// <summary>״̬, 0 δ����, 1 �Ѽ���.</summary>
        public virtual int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:SupportMenu
        private int m_SupportMenu = 0;
        /// <summary>菜单支持: 0 不支持 | 1 支持</summary>
        public int SupportMenu
        {
            get { return m_SupportMenu; }
            set { m_SupportMenu = value; }
        }
        #endregion

        #region 属性:Install()
        /// <summary>��װ����</summary>
        /// <returns>������Ϣ. =0����װ�ɹ�, >0����װʧ��.</returns>
        public virtual int Install()
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

        #region 属性:Uninstall()
        /// <summary>ж�ز���</summary>
        /// <returns>������Ϣ. =0����ж�سɹ�, >0����ж��ʧ��.</returns>
        public virtual int Uninstall()
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
        public virtual int Restart()
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

        #region 属性:Command(Hashtable agrs)
        /// <summary>ִ������</summary>
        /// <returns>������Ϣ. =0����ִ�гɹ�, >0����ִ��ʧ��.</returns>
        public virtual int Command(Hashtable agrs)
        {
            throw new Exception("Oops |-_-||, the method or operation is not implemented.");
        }
        #endregion

    }
}
