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
        #region ����:Id
        /// <summary>��ʶ</summary>
        string Id { get; set; }
        #endregion

        #region ����:Name
        /// <summary>����</summary>
        string Name { get; }
        #endregion

        #region ����:Version
        /// <summary>�汾</summary>
        string Version { get; }
        #endregion

        #region ����:Author
        /// <summary>����</summary>
        string Author { get; }
        #endregion

        #region ����:Copyright
        /// <summary>��Ȩ</summary>
        string Copyright { get; }
        #endregion

        #region ����:Url
        /// <summary>������ȡ��ַ</summary>
        string Url { get; }
        #endregion

        #region ����:ThumbnailUrl
        /// <summary>����ͼ</summary>
        string ThumbnailUrl { get; }
        #endregion

        #region ����:Description
        /// <summary>������Ϣ</summary>
        string Description { get; }
        #endregion

        #region ����:Status
        /// <summary>״̬: 0 �ر� | 1 ����</summary>
        int Status { get; set; }
        #endregion

        #region ����:Install()
        /// <summary>��װ����</summary>
        /// <returns>������Ϣ. =0����װ�ɹ�, >0����װʧ��.</returns>
        int Install();
        #endregion

        #region ����:Uninstall()
        /// <summary>ж�ز���</summary>
        /// <returns>������Ϣ. =0����ж�سɹ�, >0����ж��ʧ��.</returns>
        int Uninstall();
        #endregion

        #region ����:Restart()
        /// <summary>��������</summary>
        /// <returns>������Ϣ. =0����ж�سɹ�, >0����ж��ʧ��.</returns>
        int Restart();
        #endregion

        #region ����:Command(Hashtable agrs)
        /// <summary>ִ������</summary>
        /// <returns>������Ϣ. =0����ִ�гɹ�, >0����ִ��ʧ��.</returns>
        int Command(Hashtable agrs);
        #endregion
    }
}
