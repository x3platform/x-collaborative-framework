#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.BLL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;

  using X3Platform.Spring;

  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Plugins.Bugs.IBLL;
  using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Data;
  #endregion

  public sealed class BugHistoryService : IBugHistoryService
  {
    private BugConfiguration configuration = null;

    private IBugHistoryProvider provider = null;

    public BugHistoryService()
    {
      this.configuration = BugConfigurationView.Instance.Configuration;

      // �������󹹽���(Spring.NET)
      string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(BugConfiguration.ApplicationName, springObjectFile);

      // ���������ṩ��
      this.provider = objectBuilder.GetObject<IBugHistoryProvider>(typeof(IBugHistoryProvider));
    }

    public BugHistoryInfo this[string index]
    {
      get { return this.FindOne(index); }
    }

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(AccountInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">AccountInfo ʵ����ϸ��Ϣ</param>
    /// <param name="message">���ݿ�������ص������Ϣ</param>
    /// <returns>AccountInfo ʵ����ϸ��Ϣ</returns>
    public BugHistoryInfo Save(BugHistoryInfo param)
    {
      return this.provider.Save(param);
    }
    #endregion

    #region ����:Delete(string ids)
    /// <summary>ɾ����¼</summary>
    /// <param name="keys">��ʶ,����Զ��Ÿ���</param>
    public void Delete(string ids)
    {
      this.provider.Delete(ids);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(int id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">AccountInfo Id��</param>
    /// <returns>����һ�� AccountInfo ʵ������ϸ��Ϣ</returns>
    public BugHistoryInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugHistoryInfo> FindAll()
    {
      return FindAll(string.Empty);
    }
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugHistoryInfo> FindAll(string whereClause)
    {
      return FindAll(whereClause, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugHistoryInfo> FindAll(string whereClause, int length)
    {
      return this.provider.FindAll(whereClause, length);
    }
    #endregion

    // -------------------------------------------------------
    // �Զ��幦��
    // -------------------------------------------------------

    #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>��ҳ����</summary>
    /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
    /// <param name="pageSize">ҳ���С</param>
    /// <param name="whereClause">WHERE ��ѯ����</param>
    /// <param name="orderBy">ORDER BY ��������</param>
    /// <param name="rowCount">����</param>
    /// <returns>����һ���б�ʵ��</returns> 
    public IList<BugHistoryInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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
      return this.provider.IsExist(id);
    }
    #endregion

    // -------------------------------------------------------
    // Ȩ��
    // -------------------------------------------------------

    #region ����:GetAuthorizationObject(BugHistoryInfo param)
    /// <summary>��֤�����Ȩ��</summary>
    /// <param name="param">����֤�Ķ���</param>
    /// <returns>����</returns>
    private BugHistoryInfo GetAuthorizationObject(BugHistoryInfo param)
    {
      return param;
    }
    #endregion
  }
}
