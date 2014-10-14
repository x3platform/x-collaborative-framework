#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
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
#endregion

namespace X3Platform.Security.Authority.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    
    using Common.Logging;

    using X3Platform.CacheBuffer;
    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Security.Authority.Configuration;
    using X3Platform.Security.Authority.IBLL;
    using X3Platform.Security.Authority.IDAL;
    #endregion

    /// <summary>Ȩ�޷���</summary>
    public class AuthorityService : IAuthorityService
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����</summary>
        private AuthorityConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IAuthorityProvider provider = null;

        private IDictionary<string, AuthorityInfo> dictionary = new Dictionary<string, AuthorityInfo>();

        private DateTime actionTime = DateTime.Now;

        #region ���캯��:AuthorityService()
        /// <summary>
        /// ���캯��:AuthorityService()
        /// </summary>
        public AuthorityService()
        {
            configuration = AuthorityConfigurationView.Instance.Configuration;

            this.provider = SpringContext.Instance.GetObject<IAuthorityProvider>(typeof(IAuthorityProvider));
        }
        #endregion

        #region ����:this[string name]
        /// <summary>����</summary>
        /// <param name="name">Ȩ������</param>
        /// <returns></returns>
        public AuthorityInfo this[string name]
        {
            get
            {
                AuthorityInfo authority = this.FindOneByName(name);

                if (logger.IsDebugEnabled) { logger.Debug(authority); }

                if (authority == null)
                {
                    throw new NullReferenceException("δ�ҵ�[" + name + "]Ȩ����Ϣ.");
                }

                return authority;
            }
        }
        #endregion

        //-------------------------------------------------------
        // ���� ɾ��
        //-------------------------------------------------------

        #region ����:Save(AuthorityInfo param)
        /// <summary>������¼</summary>
        /// <param name="param"> ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
        /// <returns>AuthorityInfo ʵ����ϸ��Ϣ</returns>
        public AuthorityInfo Save(AuthorityInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">AuthorityInfo ʵ���ı�ʶ,�����Զ��ŷֿ�.</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        //-------------------------------------------------------
        // ��ѯ
        //-------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">AuthorityInfo id��</param>
        /// <returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public AuthorityInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region ����:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">Ȩ������</param>
        /// <returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public AuthorityInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        public IList<AuthorityInfo> FindAll()
        {
            return this.FindAll(new DataQuery());
        }
        #endregion

        #region ����:FindAll(DataQuery query)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <returns>��������ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        public IList<AuthorityInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        //-------------------------------------------------------
        // �Զ��幦��
        //-------------------------------------------------------

        #region ����:Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="AuthorityInfo"/></returns> 
        public IList<AuthorityInfo> Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.Query(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion
    }
}
