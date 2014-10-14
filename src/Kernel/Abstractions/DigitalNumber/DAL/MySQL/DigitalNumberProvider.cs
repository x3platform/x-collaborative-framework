// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.DigitalNumber.DAL.MySQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.IDAL;
    using X3Platform.DigitalNumber.Model;
    using X3Platform.Data;

    [DataObject]
    public class DigitalNumberProvider : IDigitalNumberProvider
    {
        /// <summary>����</summary>
        private DigitalNumberConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_DigitalNumber";

        public DigitalNumberProvider()
        {
            configuration = DigitalNumberConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }

        public DigitalNumberInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(DigitalNumberInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">DigitalNumberInfo ʵ����ϸ��Ϣ</param>
        /// <returns>DigitalNumberInfo ʵ����ϸ��Ϣ</returns>
        public DigitalNumberInfo Save(DigitalNumberInfo param)
        {
            if (!IsExistName(param.Name))
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

        #region ����:Insert(DigitalNumberInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">DigitalNumberInfo ʵ������ϸ��Ϣ</param>
        public void Insert(DigitalNumberInfo param)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region ����:Update(DigitalNumberInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">DigitalNumberInfo ʵ������ϸ��Ϣ</param>
        public void Update(DigitalNumberInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">DigitalNumberInfo Id��</param>
        /// <returns>����һ�� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        public DigitalNumberInfo FindOne(string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Name", name);

            return this.ibatisMapper.QueryForObject<DigitalNumberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        public IList<DigitalNumberInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<DigitalNumberInfo> list = this.ibatisMapper.QueryForList<DigitalNumberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        public IList<DigitalNumberInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            IList<DigitalNumberInfo> list = this.ibatisMapper.QueryForList<DigitalNumberInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }

        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) { throw new Exception("ʵ�����Ʋ���Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }

        #region ����:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>����ǰ׺�������ֱ���</summary>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="prefixCode">ǰ׺����</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        {
            // ��ȡǰ׺
            string prefix = DigitalNumberScript.RunPrefixScript(expression, prefixCode.ToUpper(), DateTime.Now);

            // ����ǰ׺��Ϣ��ѯ��ǰ�����ı���
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityTableName", entityTableName);
            args.Add("Prefix", prefix);

            // args.Add("EntityTableName", entityTableName);

            int seed = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMaxSeedByPrefix", tableName)), args);

            return DigitalNumberScript.RunScript(expression, prefixCode, DateTime.Now, ref seed);
        }
        #endregion

        #region ����:GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        /// <summary>����ǰ׺�������ֱ���</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="prefixCode">ǰ׺����</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        {
            // ��ȡǰ׺
            string prefix = DigitalNumberScript.RunPrefixScript(expression, prefixCode.ToUpper(), DateTime.Now);

            // ����ǰ׺��Ϣ��ѯ��ǰ�����ı���
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityTableName", entityTableName);
            args.Add("Prefix", prefix);

            string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMaxSeedByPrefix", tableName)), args);

            int seed = Convert.ToInt32(command.ExecuteScalar(commandText));

            return DigitalNumberScript.RunScript(expression, prefixCode, DateTime.Now, ref seed);
        }
        #endregion

        #region ����:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>����������ʶ�����ֱ���</summary>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="entityCategoryTableName">ʵ���������ݱ�</param>
        /// <param name="entityCategoryId">ʵ��������ʶ</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityCategoryTableName", entityCategoryTableName);
            args.Add("EntityCategoryId", entityCategoryId);

            string prefixCode = (string)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPrefixCodeByCategoryId", tableName)), args);

            return GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);
        }
        #endregion

        #region ����:GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>����������ʶ�����ֱ���</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="entityCategoryTableName">ʵ���������ݱ�</param>
        /// <param name="entityCategoryId">ʵ��������ʶ</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("EntityCategoryTableName", entityCategoryTableName);
            args.Add("EntityCategoryId", entityCategoryId);

            string commandText = this.ibatisMapper.QueryForCommandText(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPrefixCodeByCategoryId", tableName)), args);

            string prefixCode = (string)command.ExecuteScalar(commandText);

            return GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);
        }
        #endregion
    }
}