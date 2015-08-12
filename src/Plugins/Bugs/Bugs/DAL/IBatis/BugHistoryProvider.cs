namespace X3Platform.Plugins.Bugs.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Plugins.Bugs.Configuration;
  using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Plugins.Bugs.IDAL;
  using X3Platform.Data;
  #endregion

  [DataObject]
  public class BugHistoryProvider : IBugHistoryProvider
  {
    /// <summary>����</summary>
    private BugConfiguration configuration = null;

    /// <summary>IBatisӳ���ļ�</summary>
    private string ibatisMapping = null;

    /// <summary>IBatisӳ�����</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>���ݱ���</summary>
    private string tableName = "tb_Bug_History";

    public BugHistoryProvider()
    {
      this.configuration = BugConfigurationView.Instance.Configuration;

      this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }

    // -------------------------------------------------------
    // ���� ��� �޸� ɾ��
    // -------------------------------------------------------

    #region ����:Save(BugHistoryInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">BugHistoryInfo ʵ����ϸ��Ϣ</param>
    /// <returns>BugHistoryInfo ʵ����ϸ��Ϣ</returns>
    public BugHistoryInfo Save(BugHistoryInfo param)
    {
      if (this.IsExist(param.Id))
      {
        new Exception("The same entity's id already exists. |-_-||");
      }

      param.AccountId = KernelContext.Current.User.Id;

      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Save", this.tableName)), param);

      return param;
    }
    #endregion

    #region ����:Delete(string ids)
    /// <summary>ɾ����¼</summary>
    /// <param name="ids">��ʶ,����Զ��Ÿ���</param>
    public void Delete(string ids)
    {
      if (string.IsNullOrEmpty(ids))
        return;

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="param">BugHistoryInfo Id��</param>
    /// <returns>����һ�� BugHistoryInfo ʵ������ϸ��Ϣ</returns>
    public BugHistoryInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return this.ibatisMapper.QueryForObject<BugHistoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� BugHistoryInfo ʵ������ϸ��Ϣ</returns>
    public IList<BugHistoryInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<BugHistoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
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
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", query.GetWhereSql());
      args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<BugHistoryInfo> list = this.ibatisMapper.QueryForList<BugHistoryInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

      return list;
    }
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="id">BugHistoryInfo ʵ����ϸ��Ϣ</param>
    /// <returns>����ֵ</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion
  }
}
