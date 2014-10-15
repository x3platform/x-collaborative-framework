#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
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

namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>Ӧ�����ӷ���������Ϣ</summary>
    public class ConnectAccessTokenInfo
    {
        public ConnectAccessTokenInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>Ӧ�ñ�ʶ</summary>
        public string AppKey
        {
            get { return this.m_AppKey; }
            set { this.m_AppKey = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>�ύ�˱�ʶ</summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region 属性:ExpireDate
        private DateTime m_ExpireDate;

        /// <summary>����ʱ��</summary>
        public DateTime ExpireDate
        {
            get { return this.m_ExpireDate; }
            set { this.m_ExpireDate = value; }
        }
        #endregion

        #region 属性:ExpiresIn
        /// <summary>����ʱ��(��λ:��)</summary>
        public double ExpiresIn
        {
            get { return new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(this.ExpireDate.Ticks)).Duration().TotalSeconds; }
        }
        #endregion

        #region 属性:RefreshToken
        private string m_RefreshToken;

        /// <summary>ˢ������</summary>
        public string RefreshToken
        {
            get { return this.m_RefreshToken; }
            set { this.m_RefreshToken = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>�޸�����</summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>��������</summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
