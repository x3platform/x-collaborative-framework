using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Location.IPQuery
{
    /// <summary>IP查询的详细信息</summary>
    public class IPAddressQueryInfo
    {
        public IPAddressQueryInfo() { }

        #region 属性:Id
        private int m_Id;

        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            set { m_Id = value; }
            get { return m_Id; }
        }
        #endregion

        #region 属性:Address
        private string m_Address;

        /// <summary>
        /// [扩展]IP地址
        /// </summary>
        public string Address
        {
            set { m_Address = value; }
            get { return m_Address; }
        }
        #endregion

        #region 属性:Domain
        private string m_Domain;
        /// <summary>
        /// [扩展]域名
        /// </summary>
        public string Domain
        {
            set { m_Domain = value; }
            get { return m_Domain; }
        }
        #endregion

        #region 属性:StartIP
        private string m_StartIP;

        /// <summary>
        /// 开始IP地址
        /// </summary>
        public string StartIP
        {
            set { m_StartIP = value; }
            get { return m_StartIP; }
        }
        #endregion

        #region 属性:EndIP
        private string m_EndIP;

        /// <summary>
        /// 结束地址
        /// </summary>
        public string EndIP
        {
            set { m_EndIP = value; }
            get { return m_EndIP; }
        }
        #endregion

        #region 属性:Country
        private string m_Country;

        /// <summary>
        /// 国家
        /// </summary>
        public string Country
        {
            set { m_Country = value; }
            get { return m_Country; }
        }
        #endregion

        #region 属性:Description
        private string m_Description;

        /// <summary>描述</summary>
        public string Description
        {
            set { this.m_Description = value; }
            get { return this.m_Description; }
        }
        #endregion

        #region 属性:StartId
        private float m_StartId;

        /// <summary>
        /// 
        /// </summary>
        public float StartId
        {
            set { m_StartId = value; }
            get { return m_StartId; }
        }
        #endregion

        #region 属性:EndId
        private float m_EndId;

        /// <summary>
        /// 
        /// </summary>
        public float EndId
        {
            set { m_EndId = value; }
            get { return m_EndId; }
        }
        #endregion

        #region 属性:Province
        private string m_Province;

        /// <summary>
        /// 
        /// </summary>
        public string Province
        {
            set { m_Province = value; }
            get { return m_Province; }
        }
        #endregion

        #region 属性:City
        private string m_City;

        /// <summary>
        /// 城市
        /// </summary>
        public string City
        {
            set { m_City = value; }
            get { return m_City; }
        }
        #endregion

        #region 属性:Language
        private string m_Language;

        /// <summary>语言</summary>
        public string Language
        {
            set { m_Language = value; }
            get { return m_Language; }
        }
        #endregion
    }
}
