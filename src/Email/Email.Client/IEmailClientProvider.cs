//=============================================================================
//
// Copyright (c) 2007 X3Platform
//
// FileName     :
//
// Description  :
//
// Author       :X3Platform
//
// Date         :2007-07-08
//
//=============================================================================

using System.Net.Mail;

using X3Platform.Spring;

namespace X3Platform.Email.Client
{
    /// <summary></summary>
    [SpringObject("X3Platform.Email.Client.IEmailClientProvider")]
    public interface IEmailClientProvider
    {
        #region ����:Send(MailMessage mail)
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="mail">�ʼ�����</param>
        void Send(MailMessage mail);
        #endregion

        #region ����:Send(string toAddress, string subject, string content)
        /// <summary>Ĭ�Ϸ��͵�ַ�����ʼ�</summary>
        /// <param name="toAddress">���յ�ַ</param>
        /// <param name="subject">����</param>
        /// <param name="content">����</param>
        void Send(string toAddress, string subject, string content);
        #endregion

        #region ����:Send(string fromAddress, string toAddress, string subject, string content)
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="fromAddress">���͵�ַ</param>
        /// <param name="toAddress">���յ�ַ</param>
        /// <param name="subject">����</param>
        /// <param name="content">����</param>
        void Send(string fromAddress, string toAddress, string subject, string content);
        #endregion

        #region ����:Send(string toAddress, string subject, string content, EmailFormat format)
        /// <summary>Ĭ�Ϸ��͵�ַ�����ʼ�</summary>
        /// <param name="toAddress">���յ�ַ</param>
        /// <param name="subject">����</param>
        /// <param name="content">����</param>
        /// <param name="format">�ʼ���ʽ</param>
        void Send(string toAddress, string subject, string content, EmailFormat format);
        #endregion

        #region ����:Send(string fromAddress, string toAddress, string subject, string content, EmailFormat format)
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="fromAddress">���͵�ַ</param>
        /// <param name="toAddress">���յ�ַ</param>
        /// <param name="subject">����</param>
        /// <param name="content">����</param>
        /// <param name="format">�ʼ���ʽ</param>
        void Send(string fromAddress, string toAddress, string subject, string content, EmailFormat format);
        #endregion
    }
}
