using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Location.IPQuery
{
    /// <summary>IP��ѯ����ϸ��Ϣ</summary>
    public class IPAddressQueryInfo
    {
        public IPAddressQueryInfo() { }

        #region ����:Id
        private int m_Id;

        /// <summary>
        /// ���
        /// </summary>
        public int Id
        {
            set { m_Id = value; }
            get { return m_Id; }
        }
        #endregion

        #region ����:Address
        private string m_Address;

        /// <summary>
        /// [��չ]IP��ַ
        /// </summary>
        public string Address
        {
            set { m_Address = value; }
            get { return m_Address; }
        }
        #endregion

        #region ����:Domain
        private string m_Domain;
        /// <summary>
        /// [��չ]����
        /// </summary>
        public string Domain
        {
            set { m_Domain = value; }
            get { return m_Domain; }
        }
        #endregion

        #region ����:StartIP
        private string m_StartIP;

        /// <summary>
        /// ��ʼIP��ַ
        /// </summary>
        public string StartIP
        {
            set { m_StartIP = value; }
            get { return m_StartIP; }
        }
        #endregion

        #region ����:EndIP
        private string m_EndIP;

        /// <summary>
        /// ������ַ
        /// </summary>
        public string EndIP
        {
            set { m_EndIP = value; }
            get { return m_EndIP; }
        }
        #endregion

        #region ����:Country
        private string m_Country;

        /// <summary>
        /// ����
        /// </summary>
        public string Country
        {
            set { m_Country = value; }
            get { return m_Country; }
        }
        #endregion

        #region ����:Description
        private string m_Description;

        /// <summary>����</summary>
        public string Description
        {
            set { this.m_Description = value; }
            get { return this.m_Description; }
        }
        #endregion

        #region ����:StartId
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

        #region ����:EndId
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

        #region ����:Province
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

        #region ����:City
        private string m_City;

        /// <summary>
        /// ����
        /// </summary>
        public string City
        {
            set { m_City = value; }
            get { return m_City; }
        }
        #endregion

        #region ����:Language
        private string m_Language;

        /// <summary>����</summary>
        public string Language
        {
            set { m_Language = value; }
            get { return m_Language; }
        }
        #endregion
    }
}
