// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :DigitalNumberInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.DigitalNumber.Model
{
    /// <summary>���ֱ�����Ϣ</summary>
    public class DigitalNumberInfo
    {
        #region ���캯��:DigitalNumberInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public DigitalNumberInfo() { }
        #endregion

        #region ����:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region ����:Expression
        private string m_Expression;

        /// <summary></summary>
        public string Expression
        {
            get { return m_Expression; }
            set { m_Expression = value; }
        }
        #endregion

        #region ����:Seed
        private int m_Seed;

        /// <summary></summary>
        public int Seed
        {
            get { return m_Seed; }
            set { m_Seed = value; }
        }
        #endregion

        #region ����:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region ����:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

    }
}
