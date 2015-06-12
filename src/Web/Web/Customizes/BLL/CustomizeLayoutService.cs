namespace X3Platform.Web.Customizes.BLL
{
  using System;
  using System.Collections.Generic;
  using X3Platform.Data;
  using X3Platform.Security;
  using X3Platform.Spring;
  using X3Platform.Web.Configuration;
  using X3Platform.Web.Customizes.IBLL;
  using X3Platform.Web.Customizes.IDAL;
  using X3Platform.Web.Customizes.Model;

  /// <summary>ҳ��</summary>
  [SecurityClass]
  public class CustomizeLayoutService : SecurityObject, ICustomizeLayoutService
  {
    private ICustomizeLayoutProvider provider = null;

    #region ���캯��:CustomizeLayoutService()
    /// <summary>���캯��</summary>
    public CustomizeLayoutService()
    {
      // �������󹹽���(Spring.NET)
      string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

      // ���������ṩ��
      this.provider = objectBuilder.GetObject<ICustomizeLayoutProvider>(typeof(ICustomizeLayoutProvider));
    }
    #endregion

    #region ����:this[string index]
    /// <summary>����</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CustomizeLayoutInfo this[string index]
    {
      get { return this.FindOne(index); }
    }
    #endregion

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(CustomizeLayoutInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ����ϸ��Ϣ</param>
    /// <returns>CustomizeLayoutInfo ʵ����ϸ��Ϣ</returns>
    public CustomizeLayoutInfo Save(CustomizeLayoutInfo param)
    {
      return this.provider.Save(param);
    }
    #endregion

    #region ����:Delete(string id)
    /// <summary>ɾ����¼</summary>
    /// <param name="id">��ʶ</param>
    public void Delete(string id)
    {
      this.provider.Delete(id);
    }
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">CustomizeLayoutInfo Id��</param>
    /// <returns>����һ�� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public CustomizeLayoutInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region ����:FindOneByName(string name)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����һ�� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public CustomizeLayoutInfo FindOneByName(string name)
    {
      return this.provider.FindOneByName(name);
    }
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>�������� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeLayoutInfo> FindAll()
    {
      return this.provider.FindAll(string.Empty, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>�������� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeLayoutInfo> FindAll(string whereClause)
    {
      return this.provider.FindAll(whereClause, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeLayoutInfo> FindAll(string whereClause, int length)
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
    /// <param name="query">���ݲ�ѯ����</param>
    /// <param name="rowCount">��¼����</param>
    /// <returns>����һ�� WorkflowCollectorInfo �б�ʵ��</returns>
    public IList<CustomizeLayoutInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
    {
      return this.provider.GetPaging(startIndex, pageSize, query, out  rowCount);
    }
    #endregion

    #region ����:IsExist(string id)
    /// <summary>����Ƿ������صļ�¼</summary>
    /// <param name="id">������ʶ</param>
    /// <returns>����ֵ</returns>
    public bool IsExist(string id)
    {
      return this.provider.IsExist(id);
    }
    #endregion

    #region ����:IsExistName(string name)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����ֵ</returns>
    public bool IsExistName(string name)
    {
      return this.provider.IsExistName(name);
    }
    #endregion

    #region ����:GetHtml(string name)
    /// <summary>��ȡHtml�ı�</summary>
    /// <param name="name">����ģ������</param>
    /// <returns>Html�ı�</returns>
    public string GetHtml(string name)
    {
      return this.provider.GetHtml(name);
    }
    #endregion
  }
}
