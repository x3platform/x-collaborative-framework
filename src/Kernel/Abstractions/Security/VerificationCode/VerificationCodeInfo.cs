namespace X3Platform.Security.VerificationCode
{
    #region Using Libraries
    using System;
    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>验证码信息</summary>
    public class VerificationCodeInfo
    {
        /// <summary>构造函数</summary>
        public VerificationCodeInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:IP
        private string m_IP;

        /// <summary>IP</summary>
        public string IP
        {
            get { return this.m_IP; }
            set { this.m_IP = value; }
        }
        #endregion

        #region 属性:ObjectType
        private string m_ObjectType;

        /// <summary>对象类型: SessionId 会话标识, AccountId 帐号标识, PhoneNumber 电话号码</summary>
        public string ObjectType
        {
            get { return this.m_ObjectType; }
            set { this.m_ObjectType = value; }
        }
        #endregion

        #region 属性:ObjectValue
        private string m_ObjectValue;

        /// <summary>对象的值</summary>
        public string ObjectValue
        {
            get { return m_ObjectValue; }
            set { m_ObjectValue = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary>验证码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:ValidationType
        private string m_ValidationType;

        /// <summary>验证类型: Login 登录, ForgetPassword 忘记密码</summary>
        public string ValidationType
        {
            get { return m_ValidationType; }
            set { m_ValidationType = value; }
        }
        #endregion

        #region 属性:Expires
        private int m_Expires;

        /// <summary>验证码的有效时间, 单位:秒 second</summary>
        public int Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
        
        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建时间</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion
    }
}
