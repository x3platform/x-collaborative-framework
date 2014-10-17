namespace X3Platform.Docs
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>文档状态</summary>
    [Flags]
    public enum DocStatus
    {
        /// <summary>草稿(无工作流状态 新建:草稿状态, 撤回:删除相关流程实例, 变更为草稿状态)</summary>
        Draft = 1,
        /// <summary>已发布(新版本:已发布 => 已过期)</summary>
        Published = 2,
        /// <summary>已过期(历史版本)</summary>
        Overdue = 4,
        /// <summary>废弃</summary>
        Abandon = 8,
        /// <summary>会签中(工作流)</summary>
        Review = 16,
        /// <summary>校稿前草稿</summary>
        ProofreadDraft = 32,
        /// <summary>校稿中(工作流)</summary>
        Proofreading = 64,
        /// <summary>审批中(工作流)</summary>
        Approving = 256,
        /// <summary>被驳回(工作流)</summary>
        Rejected = 5120,
        /// <summary>未知状态</summary>
        Unkown = 10240,
    }
}
