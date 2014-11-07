namespace X3Platform.Apps.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Xsl;

    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;

    using X3Platform.Apps.Configuration;
    using X3Platform.Apps.IBLL;
    using X3Platform.Apps.IDAL;
    using X3Platform.Apps.Model;
    #endregion

    /// <summary></summary>
    public class ApplicationPackageService : IApplicationPackageService
    {
        /// <summary>配置</summary>
        private AppsConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IApplicationPackageProvider provider = null;

        #region 构造函数:ApplicationPackageService()
        /// <summary>构造函数</summary>
        public ApplicationPackageService()
        {
            this.configuration = AppsConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AppsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IApplicationPackageProvider>(typeof(IApplicationPackageProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationPackageInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ApplicationPackageInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="ApplicationPackageInfo"/>详细信息</param>
        /// <returns>实例<see cref="ApplicationPackageInfo"/>详细信息</returns>
        public ApplicationPackageInfo Save(ApplicationPackageInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">实例的标识,多条记录以逗号分开</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        #region 函数:DeleteAll()
        /// <summary>删除所有输出同步数据包记录</summary>
        public void DeleteAll()
        {
            // 1.删除数据库物理文件路径记录
            this.provider.DeleteAll();

            // 2.删除硬盘上物理文件
            string path = string.Format(@"{0}\output\{1}\",
                MembershipConfigurationView.Instance.PackageStoragePathRoot,
                MembershipConfigurationView.Instance.PackageStorageOutputApplicationId.Replace("-", string.Empty));

            if (Directory.Exists(path))
            {
                DirectoryHelper.Delete(path);
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        public ApplicationPackageInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        public IList<ApplicationPackageInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        public IList<ApplicationPackageInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="ApplicationPackageInfo"/>的详细信息</returns>
        public IList<ApplicationPackageInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="ApplicationPackageInfo"/></returns>
        public IList<ApplicationPackageInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:GetLatestPackage(string applicationId, string direction)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的编码</param>
        public ApplicationPackageInfo GetLatestPackage(string applicationId, string direction)
        {
            return provider.GetLatestPackage(applicationId, direction);
        }
        #endregion

        #region 函数:GetPackage(string applicationId, string direction, int code)
        /// <summary>查找数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="direction">数据包的流向: In | Out</param>
        /// <param name="code">数据包的编码</param>
        public ApplicationPackageInfo GetPackage(string applicationId, string direction, int code)
        {
            return provider.GetPackage(applicationId, direction, code);
        }
        #endregion

        #region 函数:CreateReceivedPackage(string applicationId, string id, string code, string path, XmlDocument doc)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="code">数据包的编码</param>
        /// <param name="path">数据包的物理路径</param>
        /// <param name="doc">数据包的详细数据</param>
        public int CreateReceivedPackage(string applicationId, int code, string path, XmlDocument doc)
        {
            return CreateReceivedPackage(DigitalNumberContext.Generate("Key_32DigitGuid"), applicationId, code, path, doc);
        }
        #endregion

        #region 函数:CreateReceivedPackage(string id, string applicationId, string code, string path, XmlDocument doc)
        /// <summary>创建接收到的数据包</summary>
        /// <param name="applicationId">应用标识</param>
        /// <param name="code">数据包的编码</param>
        /// <param name="path">数据包的物理路径</param>
        /// <param name="doc">数据包的详细数据</param>
        public int CreateReceivedPackage(string id, string applicationId, int code, string path, XmlDocument doc)
        {
            // 保存数据到物理地址
            doc.Save(path);

            // 记录数据包的位置
            ApplicationPackageInfo param = new ApplicationPackageInfo();

            param.Id = id;

            param.ApplicationId = applicationId;
            param.Code = code;
            param.ParentCode = code - 1 > 0 ? code - 1 : 0; // 编号从1开始 每次编号变化自增1
            param.Path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            param.Direction = "In";
            param.BeginDate = new DateTime(2000, 1, 1);
            param.EndDate = new DateTime(2000, 1, 1);

            try
            {
                XmlElement element = (XmlElement)doc.SelectSingleNode("/package");

                // 数据包类型
                param.Type = XmlHelper.FetchNodeAttribute(element, "packageType");
                // 数据包的开始时间
                param.BeginDate = Convert.ToDateTime(XmlHelper.FetchNodeAttribute(element, "beginDate"));
                // 数据包的结束时间
                param.EndDate = Convert.ToDateTime(XmlHelper.FetchNodeAttribute(element, "endDate"));
            }
            catch
            {
                param.BeginDate = new DateTime(2000, 1, 1);

                param.EndDate = new DateTime(2000, 1, 1);
            }

            Save(param);

            return 0;
        }
        #endregion

        #region 函数:TransformToFriendlyXml(string id)
        /// <summary>转换为友好的Xml格式</summary>
        /// <param name="id">数据包的标识</param>
        public XmlDocument TransformToFriendlyXml(string id)
        {
            ApplicationPackageInfo package = AppsContext.Instance.ApplicationPackageService[id];

            if (package == null)
            {
                throw new NullReferenceException("为找到相关数据包信息。");
            }

            if (!File.Exists(package.Path))
            {
                throw new FileNotFoundException(package.Path);
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(package.Path);

            // string packageType = doc.SelectSingleNode("package").Attributes["packageType"].Value;

            return doc;
        }
        #endregion

        #region 函数:TransformToFriendlyHtml(string id)
        /// <summary>转换为友好的Xml格式</summary>
        /// <param name="id">数据包的标识</param>
        public string TransformToFriendlyHtml(string id)
        {
            StringWriter stringWriter = new StringWriter();

            XmlDocument doc = TransformToFriendlyXml(id);

            string packageType = doc.SelectSingleNode("package").Attributes["packageType"].Value;

            using (XmlReader input = XmlReader.Create(new StringReader(doc.OuterXml)))
            {
                XslCompiledTransform transform = new XslCompiledTransform();

                string docXslPath = string.Format("{0}\\resources\\xsl\\Apps\\PackageTransform\\{1}.xsl", KernelConfigurationView.Instance.ApplicationPathRoot, packageType);

                transform.Load(docXslPath);

                transform.Transform(input, null, stringWriter);
            }

            return stringWriter.ToString();
        }
        #endregion
    }
}
