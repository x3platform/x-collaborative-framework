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

    /// <summary>�Զ��������ӿ�</summary>
    public interface ICustomPlugin
    {
        #region 属性:Id
        /// <summary>��ʶ</summary>
        string Id { get; set; }
        #endregion

        #region 属性:Name
        /// <summary>����</summary>
        string Name { get; }
        #endregion

        #region 属性:Version
        /// <summary>�汾</summary>
        string Version { get; }
        #endregion

        #region 属性:Author
        /// <summary>����</summary>
        string Author { get; }
        #endregion

        #region 属性:Copyright
        /// <summary>��Ȩ</summary>
        string Copyright { get; }
        #endregion

        #region 属性:Url
        /// <summary>������ȡ��ַ</summary>
        string Url { get; }
        #endregion

        #region 属性:ThumbnailUrl
        /// <summary>����ͼ</summary>
        string ThumbnailUrl { get; }
        #endregion

        #region 属性:Description
        /// <summary>������Ϣ</summary>
        string Description { get; }
        #endregion

        #region 属性:Status
        /// <summary>״̬: 0 �ر� | 1 ����</summary>
        int Status { get; set; }
        #endregion

        #region 属性:Install()
        /// <summary>��װ����</summary>
        /// <returns>������Ϣ. =0����װ�ɹ�, >0����װʧ��.</returns>
        int Install();
        #endregion

        #region 属性:Uninstall()
        /// <summary>ж�ز���</summary>
        /// <returns>������Ϣ. =0����ж�سɹ�, >0����ж��ʧ��.</returns>
        int Uninstall();
        #endregion

        #region 属性:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0����ж�سɹ�, >0����ж��ʧ��.</returns>
        int Restart();
        #endregion

        #region 属性:Command(Hashtable agrs)
        /// <summary>ִ������</summary>
        /// <returns>������Ϣ. =0����ִ�гɹ�, >0����ִ��ʧ��.</returns>
        int Command(Hashtable agrs);
        #endregion
    }
}
