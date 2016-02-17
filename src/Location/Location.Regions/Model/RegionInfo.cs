#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010, ruany@live.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Location.Regions.Model
{
    #region Using Libraries
    using System;
    using X3Platform.Util;
    #endregion

    /// <summary>������Ϣ</summary>
    public class RegionInfo
    {
        public RegionInfo() { }

        #region ����:ParentId
        /// <summary>���������ʶ</summary>
        public string ParentId
        {
            get
            {
                if (string.IsNullOrEmpty(this.Id))
                {
                    return string.Empty;
                }
                else
                {
                    string id = this.Id.TrimEnd('0');

                    if (id.Length % 2 == 1)
                    {
                        id = string.Concat(id, '0');
                    }

                    string parentId = id.Substring(0, id.Length - 2);

                    if (string.IsNullOrEmpty(parentId))
                    {
                        return "0";
                    }

                    return parentId.PadRight(6, '0');
                }
            }
        }
        #endregion

        #region ����:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region ����:Name
        private string m_Name;

        /// <summary>����</summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region ����:Path
        private string m_Path;

        /// <summary>·����Ϣ</summary>
        public string Path
        {
            get { return this.m_Path; }
            set { this.m_Path = value; }
        }
        #endregion
    }
}
