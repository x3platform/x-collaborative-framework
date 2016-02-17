namespace X3Platform.Location.Regions.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using X3Platform.Configuration;
    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Location.Regions.Configuration;
    using X3Platform.Location.Regions.Model;
    using X3Platform.Location.Regions.IBLL;
    using X3Platform.Location.Regions.IDAL;
    using X3Platform.CategoryIndexes;
    using X3Platform.CacheBuffer;
    #endregion

    public sealed class RegionService : IRegionService
    {
        private IRegionProvider provider = null;

        public RegionService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = RegionsConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(RegionsConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IRegionProvider>(typeof(IRegionProvider));
        }

        /// <summary>索引</summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        public RegionInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(RegionInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="RegionInfo"/>详细信息</param>
        /// <param name="message">数据库操作返回的相关信息</param>
        /// <returns>实例<see cref="RegionInfo"/>详细信息</returns>
        public RegionInfo Save(RegionInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new Exception("实例标识不能为空。"); }

            bool isNewObject = !this.IsExist(param.Id);

            this.provider.Save(param);

            return this.FindOne(param.Id);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(int id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回一个<see cref="RegionInfo"/>实例的详细信息</returns>
        public RegionInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 实例<see cref="RegionInfo"/>的详细信息</returns>
        public IList<RegionInfo> FindAll()
        {
            IList<RegionInfo> data = (IList<RegionInfo>)CachingManager.Get("Location-Regions");

            if (data == null)
            {
                data = this.provider.FindAll(new DataQuery() { Length = int.MaxValue });

                CachingManager.Set("Location-Regions", data);
            }

            return data;
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<RegionInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        /// <summary>获取异步生成的树</summary>
        /// <param name="treeName">树</param>
        /// <param name="parentId">父级节点标识</param>
        /// <param name="url">链接地址</param>
        /// <param name="enabledLeafClick">只允许点击叶子节点</param>
        /// <returns>树</returns>
        public DynamicTreeView GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick)
        {
            string searchPath = (parentId == "0") ? string.Empty : parentId.Replace(@"$", @"\");

            IList<RegionInfo> data = this.FindAll();

            data = data.Where(x => x.ParentId == parentId).ToList();

            IList<DynamicTreeNode> list = new List<DynamicTreeNode>(); // this.provider.GetDynamicTreeNodes(searchPath, string.Empty);

            foreach (RegionInfo d in data)
            {
                list.Add(new DynamicTreeNode() { Id = d.Id, Name = d.Name, ParentId = d.ParentId });
            }

            DynamicTreeView treeView = new DynamicTreeView(treeName, parentId);

            Dictionary<string, DynamicTreeNode> dictionary = new Dictionary<string, DynamicTreeNode>();

            foreach (DynamicTreeNode node in list)
            {
                node.ParentId = parentId;
                node.Url = url;

                if (!dictionary.ContainsKey(node.Id))
                {
                    if (node.HasChildren && enabledLeafClick)
                    {
                        node.Disabled = 1;
                    }

                    treeView.Add(node);

                    dictionary.Add(node.Id, node);
                }
            }

            return treeView;
        }
        #endregion
    }
}
