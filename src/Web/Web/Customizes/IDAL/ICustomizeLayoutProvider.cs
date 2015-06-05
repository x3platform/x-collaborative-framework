namespace X3Platform.Web.Customizes.IDAL
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.Data;
  using X3Platform.Data;
  using X3Platform.Messages;
  using X3Platform.Spring;
  using X3Platform.Web.Customizes.Model;
  #endregion

  /// <summary></summary>
  [SpringObject("X3Platform.Web.Customizes.IDAL.ICustomizeLayoutProvider")]
  public interface ICustomizeLayoutProvider
  {
    // -------------------------------------------------------
    // ����֧��
    // -------------------------------------------------------

    #region ����:BeginTransaction()
    /// <summary>��������</summary>
    void BeginTransaction();
    #endregion

    #region ����:BeginTransaction(IsolationLevel isolationLevel)
    /// <summary>��������</summary>
    /// <param name="isolationLevel">������뼶��</param>
    void BeginTransaction(IsolationLevel isolationLevel);
    #endregion

    #region ����:CommitTransaction()
    /// <summary>�ύ����</summary>
    void CommitTransaction();
    #endregion

    #region ����:RollBackTransaction()
    /// <summary>�ع�����</summary>
    void RollBackTransaction();
    #endregion

    // -------------------------------------------------------
    // ���� ��� �޸� ɾ��
    // -------------------------------------------------------

    #region ����:Save(CustomizeLayoutInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ����ϸ��Ϣ</param>
    /// <returns>CustomizeLayoutInfo ʵ����ϸ��Ϣ</returns>
    CustomizeLayoutInfo Save(CustomizeLayoutInfo param);
    #endregion

    #region ����:Insert(CustomizeLayoutInfo param)
    /// <summary>��Ӽ�¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ������ϸ��Ϣ</param>
    void Insert(CustomizeLayoutInfo param);
    #endregion

    #region ����:Update(CustomizeLayoutInfo param)
    /// <summary>�޸ļ�¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ������ϸ��Ϣ</param>
    void Update(CustomizeLayoutInfo param);
    #endregion

    #region ����:Delete(string id)
    /// <summary>ɾ����¼</summary>
    /// <param name="id">��ʶ</param>
    void Delete(string id);
    #endregion

    // -------------------------------------------------------
    // ��ѯ
    // -------------------------------------------------------

    #region ����:FindOne(string id)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="id">CustomizeLayoutInfo Id��</param>
    /// <returns>����һ�� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    CustomizeLayoutInfo FindOne(string id);
    #endregion

    #region ����:FindOneByName(string name)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����һ�� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    CustomizeLayoutInfo FindOneByName(string name);
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� CustomizeLayoutInfo ʵ������ϸ��Ϣ</returns>
    IList<CustomizeLayoutInfo> FindAll(string whereClause, int length);
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
    IList<CustomizeLayoutInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="param">CustomizeLayoutInfo ʵ����ϸ��Ϣ</param>
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
    string GetHtml(string id);
    #endregion
  }
}
