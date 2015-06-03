namespace X3Platform.Web.Customizes.Widgets
{
    #region Using Libraries
    using System;
    using System.Configuration;
    using System.Data;
    using System.Text;
    using System.Web;

    using X3Platform.Data;
    using X3Platform.Util;
    using X3Platform.Velocity;
    #endregion

    /// <summary>通用的窗口部件</summary>
    public sealed class GeneralWidget : AbstractWidget
    {
        /// <summary></summary>
        public override string ParseHtml()
        {
            try
            {
                string widgetRuntimeId = StringHelper.ToGuid();

                VelocityContext context = new VelocityContext();

                context.Put("widgetRuntimeId", widgetRuntimeId);

                context.Put("height", (this.Height == 0 ? "height:auto;" : "height:" + this.Height + "px;"));
                context.Put("width", (this.Width == 0 ? "width:auto;" : "width:" + this.Width + "px;"));

                // 设置最大行数
                int maxRowCount;

                int.TryParse(this.options["maxRowCount"], out maxRowCount);

                if (maxRowCount < 1)
                {
                    maxRowCount = 1;
                }

                if (maxRowCount > 100)
                {
                    maxRowCount = 100;
                }

                string tableName = this.options["tableName"].ToString();
                string tableColumns = this.options["tableColumns"].ToString();

                string url = this.options["url"].ToString();

                if (string.IsNullOrEmpty(tableName))
                {
                    context.Put("widgetHtml", "请配置相关数据");
                }
                else
                {
                    string commandText = " SELECT TOP " + maxRowCount + " " + tableColumns + " FROM " + this.options["tableName"] + " ORDER BY   " + this.options["orderBy"];

                    GenericSqlCommand command = new GenericSqlCommand(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString, ConfigurationManager.ConnectionStrings["ConnectionString"].ProviderName);

                    DataTable table = command.ExecuteQueryForDataTable(StringHelper.ToSafeSQL(commandText));

                    StringBuilder outString = new StringBuilder();

                    outString.Append("<div style=\"padding:0 10px 10px 10px;\" >");

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        outString.Append("<div style=\"padding:10px 0 0 0 ;\" ><a href=\"" + string.Format(url, table.Rows[i][0].ToString()) + "\" target=\"_blank\" >" + HttpUtility.HtmlEncode(table.Rows[i][1].ToString()) + "</a></div>");
                    }

                    outString.Append("</div>");

                    context.Put("widgetHtml", outString.ToString());
                }

                return VelocityManager.Instance.Merge(context, "web/customize/widgets/general.vm");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
