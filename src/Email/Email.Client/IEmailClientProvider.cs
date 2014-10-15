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
        #region 函数:Send(MailMessage mail)
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mail">邮件内容</param>
        void Send(MailMessage mail);
        #endregion

        #region 函数:Send(string toAddress, string subject, string content)
        /// <summary>默认发送地址发送邮件</summary>
        /// <param name="toAddress">接收地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        void Send(string toAddress, string subject, string content);
        #endregion

        #region 函数:Send(string fromAddress, string toAddress, string subject, string content)
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromAddress">发送地址</param>
        /// <param name="toAddress">接收地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        void Send(string fromAddress, string toAddress, string subject, string content);
        #endregion

        #region 函数:Send(string toAddress, string subject, string content, EmailFormat format)
        /// <summary>默认发送地址发送邮件</summary>
        /// <param name="toAddress">接收地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        /// <param name="format">邮件格式</param>
        void Send(string toAddress, string subject, string content, EmailFormat format);
        #endregion

        #region 函数:Send(string fromAddress, string toAddress, string subject, string content, EmailFormat format)
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromAddress">发送地址</param>
        /// <param name="toAddress">接收地址</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        /// <param name="format">邮件格式</param>
        void Send(string fromAddress, string toAddress, string subject, string content, EmailFormat format);
        #endregion
    }
}
