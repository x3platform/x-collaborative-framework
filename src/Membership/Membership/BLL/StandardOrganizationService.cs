using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IBLL;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;
using X3Platform.Configuration;
using X3Platform.Data;

namespace X3Platform.Membership.BLL
{
    /// <summary></summary>
    public class StandardOrganizationUnitService : IStandardOrganizationUnitService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IStandardOrganizationUnitProvider provider = null;

        #region 构造函数:StandardOrganizationUnitService()
        /// <summary>构造函数</summary>
        public StandardOrganizationUnitService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IStandardOrganizationUnitProvider>(typeof(IStandardOrganizationUnitProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStandardOrganizationUnitInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(IStandardOrganizationInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IStandardOrganizationUnitInfo"/>详细信息</param>
        /// <returns>实例<see cref="IStandardOrganizationUnitInfo"/>详细信息</returns>
        public IStandardOrganizationUnitInfo Save(IStandardOrganizationUnitInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IStandardOrganizationUnitInfo"/>����ϸ��Ϣ</returns>
        public IStandardOrganizationUnitInfo FindOne(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IStandardOrganizationUnitInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardOrganizationUnitInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IStandardOrganizationUnitInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardOrganizationUnitInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="IStandardOrganizationInfo"/>的详细信息</returns>
        public IList<IStandardOrganizationUnitInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllByParentId(string parentId)
        /// <summary>查询某个父节点下的所有组织单位</summary>
        /// <param name="parentId">父节标识</param>
        /// <returns>返回一个 IOrganizationInfo 实例的详细信息</returns>
        public IList<IStandardOrganizationUnitInfo> FindAllByParentId(string parentId)
        {
            return provider.FindAllByParentId(parentId);
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
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表<see cref="IOrganizationUnitInfo"/></returns>
        public IList<IStandardOrganizationUnitInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="name">标准组织名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 函数:IsExistGlobalName(string globalName)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="globalName">标准组织单位全局名称</param>
        /// <returns>布尔值</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">标准组织标识</param>
        /// <param name="name">标准组织名称</param>
        /// <returns>0:代表成功 1:代表已存在相同名称</returns>
        public int Rename(string id, string name)
        {
            return provider.Rename(id, name);
        }
        #endregion

        #region 函数:SetGlobalName(string id, string globalName)
        /// <summary>设置全局名称</summary>
        /// <param name="id">帐户标识</param>
        /// <param name="globalName">全局名称</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetGlobalName(string id, string globalName)
        {
            if (string.IsNullOrEmpty(globalName))
            {
                // 对象【${Id}】全局名称不能为空。
                return 1;
            }

            if (IsExistGlobalName(globalName))
            {
                return 2;
            }

            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 对象【${Id}】不存在。
                return 3;
            }

            return provider.SetGlobalName(id, globalName);
        }
        #endregion

        #region 函数:SetParentId(string id, string parentId)
        /// <summary>检测是否存在相关的记录</summary>
        /// <param name="id">组织标识</param>
        /// <param name="parentId">父级组织标识</param>
        /// <returns>0 操作成功 | 1 操作失败</returns>
        public int SetParentId(string id, string parentId)
        {
            // 检测是否存在对象
            if (!IsExist(id))
            {
                // 对象【${Id}】不存在。
                return 1;
            }

            // 检测是否存在对象
            if (!IsExist(parentId))
            {
                // 父级对象【${ParentId}】不存在。
                return 2;
            }

            return provider.SetGlobalName(id, parentId);
        }
        #endregion

        #region 函数:SyncFromPackPage(IStandardOrganizationUnitInfo param)
        /// <summary>同步信息</summary>
        /// <param name="param">岗位信息</param>
        public int SyncFromPackPage(IStandardOrganizationUnitInfo param)
        {
            provider.SyncFromPackPage(param);

            return 0;
        }
        #endregion
    }
}
