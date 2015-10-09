namespace X3Platform.DigitalNumber.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Resources;
    using System.Text;
    #endregion

    /// <summary>流水号信息</summary>
    public class DigitalNumberInfo
    {
        #region 构造函数:DigitalNumberInfo()
        /// <summary>默认构造函数</summary>
        public DigitalNumberInfo() { }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Expression
        private string m_Expression;

        /// <summary></summary>
        public string Expression
        {
            get { return m_Expression; }
            set { m_Expression = value; }
        }
        #endregion

        #region 属性:Seed
        private int m_Seed;

        /// <summary></summary>
        public int Seed
        {
            get { return m_Seed; }
            set { m_Seed = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion
    }
}
