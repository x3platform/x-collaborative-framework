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
  public class CustomizeWidgetZoneService : SecurityObject, ICustomizeWidgetZoneService
  {
    private ICustomizeWidgetZoneProvider provider = null;

    #region ���캯��:CustomizeWidgetZoneService()
    /// <summary>���캯��</summary>
    public CustomizeWidgetZoneService()
    {
      // �������󹹽���(Spring.NET)
      string springObjectFile = WebConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

      SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(WebConfiguration.APP_NAME_CUSTOMIZES, springObjectFile);

      // ���������ṩ��
      this.provider = objectBuilder.GetObject<ICustomizeWidgetZoneProvider>(typeof(ICustomizeWidgetZoneProvider));
    }
    #endregion

    #region ����:this[string index]
    /// <summary>����</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CustomizeWidgetInfo this[string index]
    {
      get { return this.FindOne(index); }
    }
    #endregion

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(CustomizeWidgetInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">CustomizeWidgetInfo ʵ����ϸ��Ϣ</param>
    /// <returns>CustomizeWidgetInfo ʵ����ϸ��Ϣ</returns>
    public CustomizeWidgetInfo Save(CustomizeWidgetInfo param)
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
    /// <param name="id">CustomizeWidgetInfo Id��</param>
    /// <returns>����һ�� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    public CustomizeWidgetInfo FindOne(string id)
    {
      return this.provider.FindOne(id);
    }
    #endregion

    #region ����:FindOneByName(string name)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����һ�� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    public CustomizeWidgetInfo FindOneByName(string name)
    {
      return this.provider.FindOneByName(name);
    }
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>�������� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeWidgetInfo> FindAll()
    {
      return this.provider.FindAll(string.Empty, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>�������� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeWidgetInfo> FindAll(string whereClause)
    {
      return this.provider.FindAll(whereClause, 0);
    }
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    public IList<CustomizeWidgetInfo> FindAll(string whereClause, int length)
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
    public IList<CustomizeWidgetInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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
    /// <param name="name">���򻮷�ģ������</param>
    /// <returns>Html�ı�</returns>
    public string GetHtml(string name)
    {
      return this.provider.GetHtml(name);
    }
    #endregion
  }
}
