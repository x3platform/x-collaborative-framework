namespace X3Platform.DigitalNumber.BLL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using X3Platform.Configuration;
    using X3Platform.Data;
    using X3Platform.Membership;
    using X3Platform.Security.Authority;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.DigitalNumber.Configuration;
    using X3Platform.DigitalNumber.IBLL;
    using X3Platform.DigitalNumber.IDAL;
    using X3Platform.DigitalNumber.Model;

    public class DigitalNumberService : IDigitalNumberService
    {
        private DigitalNumberConfiguration configuration = null;

        private IDigitalNumberProvider provider = null;

        public DigitalNumberService()
        {
            configuration = DigitalNumberConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IDigitalNumberProvider>(typeof(IDigitalNumberProvider));
        }

        #region 属性:this[string name]
        /// <summary>����</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DigitalNumberInfo this[string name]
        {
            get { return this.FindOne(name); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(DigitalNumberInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">DigitalNumberInfo ʵ����ϸ��Ϣ</param>
        /// <returns>DigitalNumberInfo ʵ����ϸ��Ϣ</returns>
        public DigitalNumberInfo Save(DigitalNumberInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="keys">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">DigitalNumberInfo Id��</param>
        /// <returns>����һ�� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        public DigitalNumberInfo FindOne(string name)
        {
            return provider.FindOne(name);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        public IList<DigitalNumberInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        public IList<DigitalNumberInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� DigitalNumberInfo ʵ������ϸ��Ϣ</returns>
        public IList<DigitalNumberInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <returns>����һ���б�</returns>
        public IList<DigitalNumberInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 属性:Generate(string name)

        private object lockObject = new object();

        /// <summary>�������ֱ���</summary>
        /// <param name="name">��������</param>
        /// <returns>���ֱ���</returns>
        public string Generate(string name)
        {
            string result = null;

            lock (lockObject)
            {
                DigitalNumberInfo param = FindOne(name);

                if (param == null)
                {
                    throw new Exception(string.Format("δ�ҵ�����������Ϣ������ϵ����Ա�������ر��š�{0}��������Ϣ��", name));
                }
                else
                {
                    int seed = param.Seed;

                    result = DigitalNumberScript.RunScript(param.Expression, param.UpdateDate, ref seed);

                    param.Seed = seed;

                    if (param.Name == "Key_32DigitGuid"
                        || param.Name == "Key_Guid"
                        || param.Name == "Key_Random_10"
                        || param.Name == "Key_Timestamp"
                        || param.Name == "Key_Session")
                    {
                        // ���ò���Ҫ�����ı��ź͸���ʱ���ı���
                    }
                    else
                    {
                        Save(param);
                    }

                    return result;
                }
            }
        }
        #endregion

        #region 属性:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>����ǰ׺�������ֱ���</summary>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="prefixCode">ǰ׺����</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        {
            return provider.GenerateCodeByPrefixCode(entityTableName, prefixCode, expression);
        }
        #endregion

        #region 属性:GenerateCodeByPrefixCode(string entityTableName, string prefixCode, string expression)
        /// <summary>����ǰ׺�������ֱ���</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="prefixCode">ǰ׺����</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByPrefixCode(GenericSqlCommand command, string entityTableName, string prefixCode, string expression)
        {
            return provider.GenerateCodeByPrefixCode(command, entityTableName, prefixCode, expression);
        }
        #endregion

        #region 属性:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>����������ʶ�����ֱ���</summary>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="entityCategoryTableName">ʵ���������ݱ�</param>
        /// <param name="entityCategoryId">ʵ��������ʶ</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            return provider.GenerateCodeByCategoryId(entityTableName, entityCategoryTableName, entityCategoryId, expression);
        }
        #endregion

        #region 属性:GenerateCodeByCategoryId(string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        /// <summary>����������ʶ�����ֱ���</summary>
        /// <param name="command">ͨ��SQL��������</param>
        /// <param name="entityTableName">ʵ�����ݱ�</param>
        /// <param name="entityCategoryTableName">ʵ���������ݱ�</param>
        /// <param name="entityCategoryId">ʵ��������ʶ</param>
        /// <param name="expression">��������ʽ</param>
        /// <returns>���ֱ���</returns>
        public string GenerateCodeByCategoryId(GenericSqlCommand command, string entityTableName, string entityCategoryTableName, string entityCategoryId, string expression)
        {
            return provider.GenerateCodeByCategoryId(command, entityTableName, entityCategoryTableName, entityCategoryId, expression);
        }
        #endregion
    }
}
