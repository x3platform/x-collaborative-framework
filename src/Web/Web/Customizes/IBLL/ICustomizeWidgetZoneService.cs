namespace X3Platform.Web.Customizes.IBLL
{
  #region Using Libraries
  using System.Collections.Generic;
  using X3Platform.Data;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
  #endregion

  /// <summary></summary>
  [SpringObject("X3Platform.Web.Customizes.IBLL.ICustomizeWidgetZoneService")]
  public interface ICustomizeWidgetZoneService
  {
    #region ����:this[string index]
    /// <summary>����</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    CustomizeWidgetInfo this[string index] { get; }
    #endregion

    // -------------------------------------------------------
    // ���� ɾ��
    // -------------------------------------------------------

    #region ����:Save(CustomizeWidgetInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">CustomizeWidgetInfo ʵ����ϸ��Ϣ</param>
    /// <returns>CustomizeWidgetInfo ʵ����ϸ��Ϣ</returns>
    CustomizeWidgetInfo Save(CustomizeWidgetInfo param);
    #endregion

    #region ����:Delete(string id)
    /// <summary>ɾ����¼</summary>
    /// <param name="keys">��ʶ,����Զ��Ÿ���</param>
    void Delete(string id);
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">CustomizeWidgetInfo Id��</param>
    /// <returns>����һ�� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    CustomizeWidgetInfo FindOne(string id);
    #endregion

    #region ����:FindOneByName(string name)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����һ�� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    CustomizeWidgetInfo FindOneByName(string name);
    #endregion

    #region ����:FindAll()
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <returns>�������� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    IList<CustomizeWidgetInfo> FindAll();
    #endregion

    #region ����:FindAll(string whereClause)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <returns>�������� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    IList<CustomizeWidgetInfo> FindAll(string whereClause);
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� CustomizeWidgetInfo ʵ������ϸ��Ϣ</returns>
    IList<CustomizeWidgetInfo> FindAll(string whereClause, int length);
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
    IList<CustomizeWidgetInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="id">CustomizeWidgetInfo ʵ����ϸ��Ϣ</param>
    /// <returns>����ֵ</returns>
    bool IsExist(string id);
    #endregion

    #region ����:IsExistName(string name)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����ֵ</returns>
    bool IsExistName(string name);
    #endregion

    #region ����:GetHtml(string name)
    /// <summary>��ȡHtml�ı�</summary>
    /// <param name="name">���򻮷�ģ������</param>
    /// <returns>Html�ı�</returns>
    string GetHtml(string name);
    #endregion
  }
}
