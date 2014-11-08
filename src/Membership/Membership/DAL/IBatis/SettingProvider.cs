using System;
using System.Collections.Generic;
using System.ComponentModel;

using X3Platform.IBatis.DataMapper;
using X3Platform.Util;

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;
using X3Platform.DigitalNumber;

namespace X3Platform.Membership.DAL.IBatis
{
    /// <summary></summary>
    [DataObject]
    public class SettingProvider : ISettingProvider
    {
        /// <summary>配置</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Setting";

        #region 构造函数:SettingProvider()
        /// <summary>构造函数</summary>
        public SettingProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // 添加 删除 修改
        //-------------------------------------------------------

        #region 函数:Save(SettingInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        /// <returns>实例<see cref="SettingInfo"/>详细信息</returns>
        public SettingInfo Save(SettingInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (SettingInfo)param;
        }
        #endregion

        #region 函数:Insert(SettingInfo param)
        /// <summary>添加记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        public void Insert(SettingInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Setting_Key_Code");

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(SettingInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">实例<see cref="SettingInfo"/>详细信息</param>
        public void Update(SettingInfo param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string ids)
        /// <summary>删除记录</summary>
        /// <param name="ids">标识,多个以逗号隔开.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            SettingInfo param = ibatisMapper.QueryForObject<SettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<SettingInfo> list = ibatisMapper.QueryForList<SettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 函数:FindAllBySettingGroupId(string settingGroupId, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="settingGroupId">参数分组标识</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAllBySettingGroupId(string settingGroupId, string keyword)
        {
            string whereClause = string.Format(@" ApplicationSettingGroupId = ##{0}## AND Text LIKE ##%{1}%## ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(settingGroupId), StringHelper.ToSafeSQL(keyword));

            if (string.IsNullOrEmpty(keyword))
            {
                whereClause = string.Format(@" ApplicationSettingGroupId = ##{0}## ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(settingGroupId));
            }

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAllBySettingGroupName(string settingGroupName, string keyword)
        /// <summary>根据参数分组信息查询所有相关记录</summary>
        /// <param name="settingGroupName">参数分组名称</param>
        /// <param name="keyword">文本信息关键字匹配</param>
        /// <returns>返回所有实例<see cref="SettingInfo"/>的详细信息</returns>
        public IList<SettingInfo> FindAllBySettingGroupName(string settingGroupName, string keyword)
        {
            string whereClause = string.Format(@" ApplicationSettingGroupId = ( SELECT Id FROM view_Application_SettingGroup WHERE Name = ##{0}## ) AND Text LIKE ##%{1}%## ORDER BY Text ", StringHelper.ToSafeSQL(settingGroupName), StringHelper.ToSafeSQL(keyword));

            if (string.IsNullOrEmpty(keyword))
            {
                whereClause = string.Format(@" ApplicationSettingGroupId = ( SELECT Id FROM view_Application_SettingGroup WHERE Name = ##{0}## ) ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(settingGroupName));
            }

            return FindAll(whereClause, 0);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<SettingInfo> list = ibatisMapper.QueryForList<SettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new ArgumentException("ʵ����ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 函数:GetText(string settingGroupName, string value)
        /// <summary>根据配置的值获取文本信息</summary>
        /// <param name="settingGroupName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetText(string settingGroupName, string value)
        {
            string whereClause = string.Format(@"
ApplicationId = ##{0}##
AND ApplicationSettingGroupId IN ( SELECT Id FROM view_Application_SettingGroup WHERE Name = ##{1}## )
AND Value = ##{2}##
", "00000000-0000-0000-0000-000000000100", StringHelper.ToSafeSQL(settingGroupName), StringHelper.ToSafeSQL(value));

            // 设置根级目录下的参数
            if (string.IsNullOrEmpty(settingGroupName))
            {
                whereClause = string.Format(@" ApplicationId = ##{0}##
                    AND ApplicationSettingGroupId = ##00000000-0000-0000-0000-000000000000## AND Value = ##{1}## ",
                     "00000000-0000-0000-0000-000000000100", StringHelper.ToSafeSQL(value));
            }

            IList<SettingInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? string.Empty : list[0].Text;
        }
        #endregion

        #region 函数:GetValue(string settingGroupName, string text)
        /// <summary>根据配置的文本获取值信息</summary>
        /// <param name="settingGroupName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetValue(string settingGroupName, string text)
        {
            string whereClause = string.Format(@"
ApplicationId = ##{0}##
AND ApplicationSettingGroupId IN ( SELECT Id FROM view_Application_SettingGroup WHERE Name = ##{1}## )
AND Text = ##{2}##
", "00000000-0000-0000-0000-000000000100", StringHelper.ToSafeSQL(settingGroupName), StringHelper.ToSafeSQL(text));

            // 设置根级目录下的参数
            if (string.IsNullOrEmpty(settingGroupName))
            {
                whereClause = string.Format(@" ApplicationId = ##{0}##
                    AND ApplicationSettingGroupId = ##00000000-0000-0000-0000-000000000000## AND Text = ##{1}## ",
                     "00000000-0000-0000-0000-000000000100", StringHelper.ToSafeSQL(text));
            }

            IList<SettingInfo> list = FindAll(whereClause, 0);

            return list.Count == 0 ? string.Empty : list[0].Value;
        }
        #endregion
    }
}
