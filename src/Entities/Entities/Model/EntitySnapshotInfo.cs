#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntitySnapshotInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
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
    public class EntitySnapshotInfo : IEntitySnapshotInfo
    {
        #region ���캯��:EntitySnapshotInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public EntitySnapshotInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:EntityId
        private string m_EntityId;

        /// <summary></summary>
        public string EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary></summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:SnapshotObject
        private string m_SnapshotObject;

        /// <summary></summary>
        public string SnapshotObject
        {
            get { return m_SnapshotObject; }
            set { m_SnapshotObject = value; }
        }
        #endregion

        #region 属性:Date
        private DateTime m_Date;

        /// <summary></summary>
        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }
        #endregion


        #region IEntitySnapshotInfo Members

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
