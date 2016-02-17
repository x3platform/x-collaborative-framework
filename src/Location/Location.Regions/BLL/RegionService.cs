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
            // �������󹹽���(Spring.NET)
            string springObjectFile = RegionsConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(RegionsConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IRegionProvider>(typeof(IRegionProvider));
        }

        /// <summary>����</summary>
        /// <param name="id">��ʶ</param>
        /// <returns></returns>
        public RegionInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(RegionInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <param name="message">���ݿ�������ص������Ϣ</param>
        /// <returns>ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</returns>
        public RegionInfo Save(RegionInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            bool isNewObject = !this.IsExist(param.Id);

            this.provider.Save(param);

            return this.FindOne(param.Id);
        }
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(int id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����һ��<see cref="RegionInfo"/>ʵ������ϸ��Ϣ</returns>
        public RegionInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <returns>�������� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
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
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        public IList<RegionInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">��Ա��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            return this.provider.IsExist(id);
        }
        #endregion

        #region ����:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick, bool elevatedPrivileges)
        /// <summary>��ȡ�첽���ɵ���</summary>
        /// <param name="treeName">��</param>
        /// <param name="parentId">�����ڵ��ʶ</param>
        /// <param name="url">���ӵ�ַ</param>
        /// <param name="enabledLeafClick">ֻ������Ҷ�ӽڵ�</param>
        /// <returns>��</returns>
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
