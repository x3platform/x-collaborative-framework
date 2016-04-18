namespace X3Platform.Util
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Data;
    #endregion
   
    /// <summary>分页数据辅助类</summary>
    public class PagingHelper
    {
        #region 静态函数:Create(string xml)
        /// <summary>根据Xml字符串创建对象</summary>
        public static PagingHelper Create(string xml)
        {
            xml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<root>{0}</root>", xml);

            PagingHelper paging = new PagingHelper();

            return AjaxUtil.Deserialize<PagingHelper>(paging, xml);
        }
        #endregion

        #region 静态函数:Create(string xml, string queryXml)
        /// <summary>根据Xml字符串创建对象</summary>
        public static PagingHelper Create(string xml, string queryXml)
        {
            PagingHelper paging = Create(xml);

            if (!string.IsNullOrEmpty(queryXml))
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<root>{0}</root>", queryXml));

                paging.Query.Deserialize(doc.DocumentElement);
            }

            return paging;
        }
        #endregion

        #region 构造函数:PagesHelper()
        /// <summary>构造函数: 默认 Page Size = 10 </summary>
        public PagingHelper() : this(10) { }
        #endregion

        #region 构造函数:PagesHelper(int pageSize)
        /// <summary>构造函数</summary>
        /// <param name="pageSize">每页显示的数目</param>
        public PagingHelper(int pageSize)
        {
            this.m_PageSize = pageSize;

            this.m_CurrentPage = 1;

            this.m_FirstPage = 1;

            this.m_PreviousPage = 1;

            this.m_NextPage = 2;
        }
        #endregion

        #region 函数:Parse()
        /// <summary>分析</summary>
        private void Parse()
        {
            // Get [rowIndex] [pageCount]

            if (m_PageSize > 0)
            {
                m_RowIndex = (m_CurrentPage < 1) ? 0 : (m_CurrentPage - 1) * m_PageSize;

                m_CurrentPage = (m_RowIndex < 0) ? 1 : (m_RowIndex / m_PageSize + 1);

                m_PageCount = (m_RowCount < 0) ? 1 : ((m_RowCount - 1) / m_PageSize + 1);

                m_LastPage = m_PageCount;
            }
            else
            {
                m_PageCount = 1;
            }

            // Get [previousPage] [nextPage]

            m_PreviousPage = (m_CurrentPage <= 1) ? 1 : m_CurrentPage - 1;

            m_NextPage = (m_CurrentPage >= m_PageCount) ? m_PageCount : m_CurrentPage + 1;
        }
        #endregion

        #region 属性:PageSize
        private int m_PageSize;

        /// <summary>每页显示行数</summary>
        public int PageSize
        {
            get { return m_PageSize; }
            set
            {
                if (value > 0)
                {
                    m_PageSize = value;

                    this.Parse();
                }
            }
        }
        #endregion

        #region 属性:RowIndex
        private int m_RowIndex;

        /// <summary>行索引号</summary>
        public int RowIndex
        {
            get { return m_RowIndex; }

            set
            {
                if (value > 0)
                {
                    m_RowIndex = value;

                    this.Parse();
                }
            }
        }
        #endregion

        #region 属性:RowCount
        private int m_RowCount;

        /// <summary>
        /// 行数统计.
        /// </summary>
        public int RowCount
        {
            get { return m_RowCount; }
            set
            {
                m_RowCount = value;

                this.Parse();
            }
        }
        #endregion

        #region 属性:PageCount
        private int m_PageCount;

        /// <summary>页数统计</summary>
        public int PageCount
        {
            get { return m_PageCount; }
        }
        #endregion

        #region 属性:CurrentPage
        private int m_CurrentPage;

        /// <summary>当前页</summary>
        public int CurrentPage
        {
            get { return m_CurrentPage; }
            set
            {
                if (value > 0)
                {
                    m_CurrentPage = value;

                    this.Parse();
                }
            }
        }
        #endregion

        #region 属性:FirstPage
        public int m_FirstPage;

        /// <summary>首页</summary>
        public int FirstPage
        {
            get { return m_FirstPage; }
        }
        #endregion

        #region 属性:PreviousPage
        /// <summary>上页</summary>
        private int m_PreviousPage;

        public int PreviousPage
        {
            get { return m_PreviousPage; }
        }
        #endregion

        #region 属性:NextPage
        private int m_NextPage;

        /// <summary>下页</summary>
        public int NextPage
        {
            get { return m_NextPage; }
        }
        #endregion

        #region 属性:LastPage
        private int m_LastPage;

        /// <summary>末页</summary>
        public int LastPage
        {
            get { return m_LastPage; }
        }
        #endregion

        // -------------------------------------------------------
        // 分页数据查询条件
        // -------------------------------------------------------

        #region 属性:Query
        private DataQuery m_Query = new DataQuery();

        /// <summary>数据查询对象</summary>
        public DataQuery Query
        {
            get { return this.m_Query; }
        }
        #endregion

        #region 属性:WhereClause
        private string m_WhereClause;

        /// <summary>SQL 查询条件</summary>
        [Obsolete("属性 WhereClause 将不被支持, 请使用 Query.Where 代替.")]
        public string WhereClause
        {
            get { return m_WhereClause; }
            set { m_WhereClause = value; }
        }
        #endregion

        #region 属性:OrderBy
        private string m_OrderBy;

        /// <summary>SQL 排序规则</summary>
        [Obsolete("属性 OrderBy 将不被支持, 请使用 Query.Orders 代替.")]
        public string OrderBy
        {
            get { return m_OrderBy; }
            set { m_OrderBy = value; }
        }
        #endregion

        #region 函数:ToString()
        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{\"pageSize\":\"" + this.PageSize + "\","
                + "\"rowIndex\":\"" + this.RowIndex + "\","
                + "\"rowCount\":\"" + this.RowCount + "\","
                + "\"pageCount\":\"" + this.PageCount + "\","
                + "\"firstPage\":\"" + this.FirstPage + "\","
                + "\"previousPage\":\"" + this.PreviousPage + "\","
                + "\"nextPage\":\"" + this.NextPage + "\","
                + "\"lastPage\":\"" + this.LastPage + "\"}";
        }
        #endregion
    }
}
