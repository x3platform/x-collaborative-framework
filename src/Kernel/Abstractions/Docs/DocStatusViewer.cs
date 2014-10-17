namespace X3Platform.Docs
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>文档状态查看器</summary>
    public class DocStatusViewer
    {
        public static string GetText(DocStatus statusValue)
        {
            switch (statusValue)
            {
                case DocStatus.Draft:
                    return "草稿";
                case DocStatus.Published:
                    return "已发布";
                case DocStatus.Overdue:
                    return "已过期";
                case DocStatus.Abandon:
                    return "废弃";
                case DocStatus.Review:
                    return "会签";
                case DocStatus.ProofreadDraft:
                    return "校稿前草稿";
                case DocStatus.Proofreading:
                    return "校稿中";
                case DocStatus.Approving:
                    return "审批中";
                case DocStatus.Rejected:
                    return "被驳回";
                case DocStatus.Unkown:
                default:
                    return "未知状态";
            }
        }

        public static DocStatus GetValue(string statusText)
        {
            switch (statusText)
            {
                case "草稿":
                    return DocStatus.Draft;
                case "已发布":
                    return DocStatus.Published;
                case  "已过期":
                    return DocStatus.Overdue;
                case "废弃":
                    return DocStatus.Abandon;
                case "会签":
                    return DocStatus.Review;
                case "校稿前草稿":
                    return DocStatus.ProofreadDraft;
                case "校稿中":
                    return DocStatus.Proofreading;
                case "审批中":
                    return DocStatus.Approving;
                case "被驳回":
                    return DocStatus.Rejected;
                default:
                    return DocStatus.Unkown;
            }
        }
    }
}
