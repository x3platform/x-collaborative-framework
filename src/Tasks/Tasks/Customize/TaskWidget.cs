#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.Customize
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    using X3Platform.Membership;
    using X3Platform.Velocity;
    using X3Platform.Util;
    using X3Platform.Web.Customize;

    using X3Platform.Ajax.Json;
    using X3Platform.Configuration;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary>Task Widget</summary>
    public sealed class TaskWidget : IWidget
    {
        private Dictionary<string, string> options = new Dictionary<string, string>();

        private int m_Height = 0;

        /// <summary>�߶�</summary>
        public int Height
        {
            get { return this.m_Height; }
        }

        private int m_Width = 0;

        /// <summary>����</summary>
        public int Width
        {
            get { return this.m_Width; }
        }

        /// <summary></summary>
        public void Load(string options)
        {
            JsonObject optionObjects = JsonObjectConverter.Deserialize(options);

            foreach (string key in optionObjects.Keys)
            {
                this.options.Add(key, ((JsonPrimary)optionObjects[key]).Value.ToString());
            }

            try
            {
                // ���ò����ĸ߶�
                int.TryParse(this.options["height"], out  this.m_Height);
                // ���ò����Ŀ���
                int.TryParse(this.options["width"], out  this.m_Width);
            }
            catch
            {
                this.m_Height = this.m_Width = 0;
            }
        }

        /// <summary></summary>
        public string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            context.Put("height", (this.Height == 0 ? "height:auto;" : "height:" + this.Height + "px;"));
            context.Put("width", (this.Width == 0 ? "width:auto;" : "width:" + this.Width + "px;"));

            IAccountInfo account = KernelContext.Current.User;

            Dictionary<int, int> unfinishedQuantities = TasksContext.Instance.TaskReceiverService.GetUnfinishedQuantities(account.Id);

            // �������� = (1)�������� + (8)�߰����� + (266)�Զ��߰�����
            string unfinishedQuantitie1 = unfinishedQuantities[275] > 99 ? "99+" : unfinishedQuantities[275].ToString();
            // ֪ͨ���� = (2)��Ϣ���� + (4)֪ͨ���� + (258)�Զ���Ϣ���� + (260)�Զ�֪ͨ����
            string unfinishedQuantitie2 = unfinishedQuantities[524] > 99 ? "99+" : unfinishedQuantities[524].ToString();

            IList<TaskWorkItemInfo> list = TasksContext.Instance.TaskReceiverService.GetWidgetData(account.Id, 20);

            foreach (TaskWorkItemInfo item in list)
            {
                item.Title = HttpUtility.HtmlEncode(item.Title);
            }

            context.Put("unfinishedQuantitie1", unfinishedQuantitie1);
            context.Put("unfinishedQuantitie2", unfinishedQuantitie2);

            context.Put("list", list);
            context.Put("signature", KernelConfigurationView.Instance.ApplicationClientSignature);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/tasks.vm");
        }
    }
}
