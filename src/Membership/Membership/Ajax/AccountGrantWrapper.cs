namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public sealed class AccountGrantWrapper : ContextWrapper
    {
        /// <summary>数据服务</summary>
        private IAccountGrantService service = MembershipManagement.Instance.AccountGrantService;

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(XmlDocument doc)
        /// <summary>保存记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("save")]
        public string Save(XmlDocument doc)
        {
            IAccountGrantInfo param = new AccountGrantInfo();

            param = (IAccountGrantInfo)AjaxStorageConvertor.Deserialize(param, doc);

            if (this.service.IsExistGrantor(param.GrantorId, param.GrantedTimeFrom, param.GrantedTimeTo, param.Id))
            {
                return "{message:{\"returnCode\":1,\"value\":\"委托人在委托时间段内已经存在委托信息，请修改相关设置。\"}}";
            }

            if (this.service.IsExistGrantee(param.GrantorId, param.GrantedTimeFrom, param.GrantedTimeTo, param.Id))
            {
                return "{message:{\"returnCode\":2,\"value\":\"委托人在委托时间段内已经存被委托关系，系统不允许二次委托授权，请修改相关设置。\"}}";
            }

            if (param.GrantedTimeFrom > param.GrantedTimeTo)
            {
                return "{message:{\"returnCode\":3,\"value\":\"委托的开始时间大于结束时间，请修改相关设置。\"}}";
            }

            this.service.Save(param);

            return "{message:{\"returnCode\":0,\"value\":\"保存成功。\"}}";
        }
        #endregion

        #region 函数:Delete(XmlDocument doc)
        /// <summary>删除记录</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("delete")]
        public string Delete(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            this.service.Delete(id);

            return "{message:{\"returnCode\":0,\"value\":\"删除成功。\"}}";
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindAll()
        /// <summary>查询所有数据</summary>
        /// <returns>返回一个相关的实例列表.</returns> 
        [AjaxMethod("findAll")]
        public string FindAll(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = AjaxStorageConvertor.Fetch("whereClause", doc);

            IList<IAccountGrantInfo> list = this.service.FindAll(whereClause);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(list) + ",");

            outString.Append("message:{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:FindOne(XmlDocument doc)
        /// <summary>获取对象信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        [AjaxMethod("findOne")]
        public string FindOne(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string id = AjaxStorageConvertor.Fetch("id", doc);

            IAccountGrantInfo param = this.service.FindOne(id);

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPages(XmlDocument doc)
        /// <summary>获取分页内容</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns></returns> 
        [AjaxMethod("getPages")]
        public string GetPages(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            PagingHelper pages = PagingHelper.Create(AjaxStorageConvertor.Fetch("pages", doc, "xml"));

            int rowCount = -1;

            IList<IAccountGrantInfo> list = this.service.GetPages(pages.RowIndex, pages.PageSize, pages.WhereClause, pages.OrderBy, out rowCount);

            pages.RowCount = rowCount;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(list) + ",");

            outString.Append("\"pages\":" + pages + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:CreateNewObject(XmlDocument doc)
        /// <summary>创建新的对象</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("createNewObject")]
        public string CreateNewObject(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string parentId = AjaxStorageConvertor.Fetch("parentId", doc);

            IAccountGrantInfo param = new AccountGrantInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");

            param.Status = 1;

            param.GrantedTimeFrom = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            param.GrantedTimeTo = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd 00:00:00"));

            param.UpdateDate = param.CreateDate = DateTime.Now;

            outString.Append("{\"ajaxStorage\":" + AjaxStorageConvertor.Parse<IAccountGrantInfo>(param) + ",");

            outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Abort(XmlDocument doc)
        /// <summary>中止当前委托</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        [AjaxMethod("abort")]
        public string Abort(XmlDocument doc)
        {
            string id = AjaxStorageConvertor.Fetch("id", doc);

            this.service.Abort(id);

            return "{message:{\"returnCode\":0,\"value\":\"设置成功。\"}}";
        }
        #endregion
    }
}