#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :VerificationCodeProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Security.VerificationCode.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Security.VerificationCode.Configuration;
    using X3Platform.Security.VerificationCode.IDAL;
    using X3Platform.Data;
    using Common.Logging;
    #endregion

    /// <summary></summary>
    [DataObject]
    public class VerificationCodeProvider : IVerificationCodeProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_VerificationCode";

        #region 构造函数:VerificationCodeProvider()
        /// <summary>构造函数</summary>
        public VerificationCodeProvider()
        {
            this.ibatisMapping = VerificationCodeConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // 保存 添加 修改 删除 
        //-------------------------------------------------------

        #region 函数:Save(VerificationCodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">VerificationCodeInfo 实例详细信息</param>
        /// <returns>VerificationCodeInfo 实例详细信息</returns>
        public VerificationCodeInfo Save(VerificationCodeInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 属性:Insert(VerificationCodeInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="VerificationCodeInfo"/>��ϸ��Ϣ</param>
        public void Insert(VerificationCodeInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 函数:Update(VerificationCodeInfo param)
        /// <summary>修改记录</summary>
        /// <param name="param">VerificationCodeInfo 实例的详细信息</param>
        public void Update(VerificationCodeInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">实例的标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string objectType, string objectValue, string validationType)
        /// <summary>查询某条记录</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationCodeInfo"/>实例的详细信息</returns>
        public VerificationCodeInfo FindOne(string objectType, string objectValue, string validationType)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("ObjectType", StringHelper.ToSafeSQL(objectType));
            args.Add("ObjectValue", StringHelper.ToSafeSQL(objectValue));
            args.Add("ValidationType", StringHelper.ToSafeSQL(validationType));

            return this.ibatisMapper.QueryForObject<VerificationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByObjectType", tableName)), args);
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">查询参数</param>
        /// <returns>返回所有<see cref="VerificationCodeInfo"/> 实例的详细信息</returns>
        public IList<VerificationCodeInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql());
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<VerificationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 属性:Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="VerificationCodeInfo"/></returns> 
        public IList<VerificationCodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "ObjectValue", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" CreatedDate DESC "));

            args.Add("RowCount", 0);

            IList<VerificationCodeInfo> list = this.ibatisMapper.QueryForList<VerificationCodeInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("ʵ����ʶ����Ϊ��.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion
    }
}
