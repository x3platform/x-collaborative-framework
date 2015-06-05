namespace X3Platform.Web.Customizes.DAL.IBatis
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;

  using X3Platform.IBatis.DataMapper;
  using X3Platform.Util;

  using X3Platform.Web.Configuration;
  using X3Platform.Web.Customizes.Model;
  using X3Platform.Web.Customizes.IDAL;
  using X3Platform.Data;
  #endregion

  /// <summary></summary>
  [DataObject]
  public class CustomizeLayoutProvider : ICustomizeLayoutProvider
  {
    /// <summary>IBatisӳ���ļ�</summary>
    private string ibatisMapping = null;

    /// <summary>IBatisӳ�����</summary>
    private ISqlMapper ibatisMapper = null;

    /// <summary>���ݱ���</summary>
    private string tableName = "tb_Customize_Layout";

    #region ���캯��:CustomizeLayoutProvider()
    /// <summary>���캯��</summary>
    public CustomizeLayoutProvider()
    {
      this.ibatisMapping = WebConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

      this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
    }
    #endregion

    // -------------------------------------------------------
    // ����֧��
    // -------------------------------------------------------

    #region ����:BeginTransaction()
    /// <summary>��������</summary>
    public void BeginTransaction()
    {
      this.ibatisMapper.BeginTransaction();
    }
    #endregion

    #region ����:BeginTransaction(IsolationLevel isolationLevel)
    /// <summary>��������</summary>
    /// <param name="isolationLevel">������뼶��</param>
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
      this.ibatisMapper.BeginTransaction(isolationLevel);
    }
    #endregion

    #region ����:CommitTransaction()
    /// <summary>�ύ����</summary>
    public void CommitTransaction()
    {
      this.ibatisMapper.CommitTransaction();
    }
    #endregion

    #region ����:RollBackTransaction()
    /// <summary>�ع�����</summary>
    public void RollBackTransaction()
    {
      this.ibatisMapper.RollBackTransaction();
    }
    #endregion

    // -------------------------------------------------------
    // ���� ��� �޸� ɾ��
    // -------------------------------------------------------

    #region ����:Save(CustomizeLayoutInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ����ϸ��Ϣ</param>
    /// <returns>CustomizeLayoutInfo ʵ����ϸ��Ϣ</returns>
    public CustomizeLayoutInfo Save(CustomizeLayoutInfo param)
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

    #region ����:Insert(CustomizeLayoutInfo param)
    /// <summary>��Ӽ�¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ������ϸ��Ϣ</param>
    public void Insert(CustomizeLayoutInfo param)
    {
      this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
    }
    #endregion

    #region ����:Update(CustomizeLayoutInfo param)
    /// <summary>�޸ļ�¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ������ϸ��Ϣ</param>
    public void Update(CustomizeLayoutInfo param)
    {
      this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
    }
    #endregion

    #region ����:Delete(string id)
    /// <summary>ɾ����¼</summary>
    /// <param name="id">��ʶ</param>
    public void Delete(string id)
    {
      if (string.IsNullOrEmpty(id)) { return; }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

      this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="param">CustomizeLayoutInfo Id��</param>
    /// <returns>����һ�� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public CustomizeLayoutInfo FindOne(string id)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Id", id);

      return this.ibatisMapper.QueryForObject<CustomizeLayoutInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
    }
    #endregion

    #region ����:FindOneByName(string name)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����һ�� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public CustomizeLayoutInfo FindOneByName(string name)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Name", name);

      return this.ibatisMapper.QueryForObject<CustomizeLayoutInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeLayoutInfo> FindAll(string whereClause, int length)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
      args.Add("Length", length);

      return this.ibatisMapper.QueryForList<CustomizeLayoutInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
    }
    #endregion

    // -------------------------------------------------------
    // �Զ��幦��
    // -------------------------------------------------------

    #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    /// <summary>��ҳ����</summary>
    /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
    /// <param name="pageSize">ҳ���С</param>
    /// <param name="query">���ݲ�ѯ����</param>
    /// <param name="rowCount">����</param>
    /// <returns>����һ���б�ʵ��</returns> 
    public IList<CustomizeLayoutInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      if (query.Variables["scence"] == "Query")
      {
        string searhText = StringHelper.ToSafeSQL(query.Where["SearchText"].ToString());

        if (string.IsNullOrEmpty(searhText))
        {
          args.Add("WhereClause", string.Empty);
        }
        else
        {
          args.Add("WhereClause", " Id LIKE '%" + searhText + "%' OR Name LIKE '%" + searhText + "%' ");
        }
      }
      else
      {
        args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
      }

      args.Add("OrderBy", query.GetOrderBySql(" UpdateDate DESC "));

      args.Add("StartIndex", startIndex);
      args.Add("PageSize", pageSize);

      IList<CustomizeLayoutInfo> list = this.ibatisMapper.QueryForList<CustomizeLayoutInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

      rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

      return list;
    }
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="id">CustomizeLayoutInfo ʵ����ϸ��Ϣ</param>
    /// <returns>����ֵ</returns>
    public bool IsExist(string id)
    {
      if (string.IsNullOrEmpty(id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region ����:IsExistName(string name)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����ֵ</returns>
    public bool IsExistName(string name)
    {
      if (string.IsNullOrEmpty(name)) { throw new Exception("���Ʋ���Ϊ�ա�"); }

      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("WhereClause", string.Format(" Name = '{1}' ", StringHelper.ToSafeSQL(name)));

      return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
    }
    #endregion

    #region ����:GetHtml(string name)
    /// <summary>��ȡHtml�ı�</summary>
    /// <param name="name">���򻮷�ģ������</param>
    /// <returns>Html�ı�</returns>
    public string GetHtml(string name)
    {
      Dictionary<string, object> args = new Dictionary<string, object>();

      args.Add("Name", StringHelper.ToSafeSQL(name));

      return this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetHtml", tableName)), args).ToString();
    }
    #endregion
  }
}
