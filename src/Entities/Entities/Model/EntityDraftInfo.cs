#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityDraftInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    public class EntityDraftInfo : IEntityDraftInfo
    {
        #region ���캯��:EntityDraftInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public EntityDraftInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:EntityId
        private string m_EntityId;

        /// <summary></summary>
        public string EntityId
        {
            get { return this.m_EntityId; }
            set { this.m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary></summary>
        public string EntityClassName
        {
            get { return this.m_EntityClassName; }
            set { this.m_EntityClassName = value; }
        }
        #endregion

        #region 属性:OriginalEntityId
        private string m_OriginalEntityId;

        /// <summary></summary>
        public string OriginalEntityId
        {
            get { return this.m_OriginalEntityId; }
            set { this.m_OriginalEntityId = value; }
        }
        #endregion

        #region 属性:Date
        private DateTime m_Date;

        /// <summary></summary>
        public DateTime Date
        {
            get { return this.m_Date; }
            set { this.m_Date = value; }
        }
        #endregion

        #region IEntityDraftInfo Members

        public string EnetiyId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ExportToXml()
        {
            throw new NotImplementedException();
        }

        public void LoadFormXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISerializedObject Members

        public void Deserialize(System.Xml.XmlElement element)
        {
            throw new NotImplementedException();
        }

        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            throw new NotImplementedException();
        }

        public string Serializable()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
