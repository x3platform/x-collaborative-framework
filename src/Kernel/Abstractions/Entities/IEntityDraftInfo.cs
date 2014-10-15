#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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

namespace X3Platform.Entities
{
    /// <summary>ʵ�����ݸ��ӿ�</summary>
    public interface IEntityDraftInfo : ISerializedObject
    {
        #region 属性:EnetiyId
        /// <summary></summary>
        string EnetiyId { get; set; }
        #endregion

        #region 属性:EntityClassName
        /// <summary></summary>
        string EntityClassName { get; set; }
        #endregion
    }
}
