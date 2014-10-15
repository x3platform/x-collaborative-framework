// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :SettingService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IBLL;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;

namespace X3Platform.Membership.BLL
{
    /// <summary></summary>
    public class SettingService : ISettingService
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private ISettingProvider provider = null;

        #region 构造函数:SettingService()
        /// <summary>构造函数</summary>
        public SettingService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ISettingProvider>(typeof(ISettingProvider));
        }
        #endregion

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SettingInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        //-------------------------------------------------------
        // 保存 删除
        //-------------------------------------------------------

        #region 函数:Save(SettingInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        /// <returns>实例<see cref="SettingInfo"/>详细信息</returns>
        public SettingInfo Save(SettingInfo param)
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

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="SettingInfo"/>的详细信息</returns>
        public SettingInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 函数:FindAllBySettingGroupId(string applicationSettingGroupId);
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupId">参数分组标识</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAllBySettingGroupId(string applicationSettingGroupId)
        {
            return provider.FindAllBySettingGroupId(applicationSettingGroupId, null);
        }
        #endregion

        #region 函数:FindAllBySettingGroupId(string applicationSettingGroupId, string keyword);
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupId">参数分组标识</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAllBySettingGroupId(string applicationSettingGroupId, string keyword)
        {
            return provider.FindAllBySettingGroupId(applicationSettingGroupId, keyword);
        }
        #endregion

        #region 函数:FindAllBySettingGroupName(string applicationSettingGroupName)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupName">参数分组名称</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAllBySettingGroupName(string applicationSettingGroupName)
        {
            return provider.FindAllBySettingGroupName(applicationSettingGroupName, null);
        }
        #endregion

        #region 函数:FindAllBySettingGroupName(string applicationSettingGroupName, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="applicationSettingGroupName">参数分组名称</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAllBySettingGroupName(string applicationSettingGroupName, string keyword)
        {
            return provider.FindAllBySettingGroupName(applicationSettingGroupName, keyword);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 函数:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="SettingInfo"/></returns>
        public IList<SettingInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
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

        #region 函数:GetText(string applicationSettingGroupName, string value)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="settingGroupName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetText(string settingGroupName, string value)
        {
            return provider.GetText(settingGroupName, value);
        }
        #endregion

        #region 函数:GetValue(string settingGroupName, string text)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="settingGroupName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetValue(string settingGroupName, string text)
        {
            return provider.GetValue(settingGroupName, text);
        }
        #endregion
    }
}
