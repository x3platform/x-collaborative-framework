// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ISettingProvider.cs
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
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Setting";

        #region ���캯��:SettingProvider()
        /// <summary>���캯��</summary>
        public SettingProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // ���� ɾ�� �޸�
        //-------------------------------------------------------

        #region 属性:Save(SettingInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="SettingInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="SettingInfo"/>��ϸ��Ϣ</returns>
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

        #region 属性:Insert(SettingInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="SettingInfo"/>��ϸ��Ϣ</param>
        public void Insert(SettingInfo param)
        {
            param.Code = DigitalNumberContext.Generate("Table_Application_Setting_Key_Code");

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(SettingInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="SettingInfo"/>��ϸ��Ϣ</param>
        public void Update(SettingInfo param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���.</param>
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
        // ��ѯ
        //-------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="SettingInfo"/>����ϸ��Ϣ</returns>
        public SettingInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            SettingInfo param = ibatisMapper.QueryForObject<SettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="SettingInfo"/>����ϸ��Ϣ</returns>
        public IList<SettingInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<SettingInfo> list = ibatisMapper.QueryForList<SettingInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:FindAllBySettingGroupId(string settingGroupId, string keyword)
        /// <summary>���ݲ���������Ϣ��ѯ�������ؼ�¼</summary>
        /// <param name="settingGroupId">����������ʶ</param>
        /// <param name="keyword">�ı���Ϣ�ؼ���ƥ��</param>
        /// <returns>��������ʵ��<see cref="SettingInfo"/>����ϸ��Ϣ</returns>
        public IList<SettingInfo> FindAllBySettingGroupId(string settingGroupId, string keyword)
        {
            string whereClause = string.Format(@" ApplicationSettingGroupId = ##{0}## AND Text LIKE ##%{1}%## ORDER BY Text ", StringHelper.ToSafeSQL(settingGroupId), StringHelper.ToSafeSQL(keyword));

            if (string.IsNullOrEmpty(keyword))
            {
                whereClause = string.Format(@" ApplicationSettingGroupId = ##{0}## ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(settingGroupId));
            }

            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAllBySettingGroupName(string settingGroupName, string keyword)
        /// <summary>���ݲ���������Ϣ��ѯ�������ؼ�¼</summary>
        /// <param name="settingGroupName">������������</param>
        /// <param name="keyword">�ı���Ϣ�ؼ���ƥ��</param>
        /// <returns>��������ʵ��<see cref="SettingInfo"/>����ϸ��Ϣ</returns>
        public IList<SettingInfo> FindAllBySettingGroupName(string settingGroupName, string keyword)
        {
            string whereClause = string.Format(@" ApplicationSettingGroupId = ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{0}## ) AND Text LIKE ##%{1}%## ORDER BY Text ", StringHelper.ToSafeSQL(settingGroupName), StringHelper.ToSafeSQL(keyword));

            if (string.IsNullOrEmpty(keyword))
            {
                whereClause = string.Format(@" ApplicationSettingGroupId = ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{0}## ) ORDER BY OrderId, Text ", StringHelper.ToSafeSQL(settingGroupName));
            }

            return FindAll(whereClause, 0);
        }
        #endregion

        //-------------------------------------------------------
        // �Զ��幦��
        //-------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="SettingInfo"/></returns>
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

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("ʵ����ʶ����Ϊ�ա�");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 属性:GetText(string settingGroupName, string value)
        /// <summary>�������õ�ֵ��ȡ�ı���Ϣ</summary>
        /// <param name="settingGroupName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetText(string settingGroupName, string value)
        {
            string whereClause = string.Format(@"
ApplicationId = ##{0}##
AND ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{1}## )
AND Value = ##{2}##
", "00000000-0000-0000-0000-000000000100", StringHelper.ToSafeSQL(settingGroupName), StringHelper.ToSafeSQL(value));

            // ���ø���Ŀ¼�µĲ���
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

        #region 属性:GetValue(string settingGroupName, string text)
        /// <summary>�������õ��ı���ȡֵ��Ϣ</summary>
        /// <param name="settingGroupName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string GetValue(string settingGroupName, string text)
        {
            string whereClause = string.Format(@"
ApplicationId = ##{0}##
AND ApplicationSettingGroupId IN ( SELECT Id FROM tb_Application_SettingGroup WHERE Name = ##{1}## )
AND Text = ##{2}##
", "00000000-0000-0000-0000-000000000100", StringHelper.ToSafeSQL(settingGroupName), StringHelper.ToSafeSQL(text));

            // ���ø���Ŀ¼�µĲ���
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
