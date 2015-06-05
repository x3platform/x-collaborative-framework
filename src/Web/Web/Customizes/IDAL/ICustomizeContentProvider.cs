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
  [SpringObject("X3Platform.Web.Customizes.IDAL.ICustomizeContentProvider")]
  public interface ICustomizeContentProvider
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

    #region ����:Save(CustomizeContentInfo param)
    /// <summary>�����¼</summary>
    /// <param name="param">CustomizeContentInfo ʵ����ϸ��Ϣ</param>
    /// <returns>CustomizeContentInfo ʵ����ϸ��Ϣ</returns>
    CustomizeContentInfo Save(CustomizeContentInfo param);
    #endregion

    #region ����:Insert(CustomizeContentInfo param)
    /// <summary>��Ӽ�¼</summary>
    /// <param name="param">CustomizeContentInfo ʵ������ϸ��Ϣ</param>
    void Insert(CustomizeContentInfo param);
    #endregion

    #region ����:Update(CustomizeContentInfo param)
    /// <summary>�޸ļ�¼</summary>
    /// <param name="param">CustomizeContentInfo ʵ������ϸ��Ϣ</param>
    void Update(CustomizeContentInfo param);
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
    /// <param name="id">CustomizeContentInfo Id��</param>
    /// <returns>����һ�� CustomizeContentInfo ʵ������ϸ��Ϣ</returns>
    CustomizeContentInfo FindOne(string id);
    #endregion

    #region ����:FindOneByName(string name)
    /// <summary>��ѯĳ����¼</summary>
    /// <param name="name">ҳ������</param>
    /// <returns>����һ�� CustomizeContentInfo ʵ������ϸ��Ϣ</returns>
    CustomizeContentInfo FindOneByName(string name);
    #endregion

    #region ����:FindAll(string whereClause,int length)
    /// <summary>��ѯ������ؼ�¼</summary>
    /// <param name="whereClause">SQL ��ѯ����</param>
    /// <param name="length">����</param>
    /// <returns>�������� CustomizeContentInfo ʵ������ϸ��Ϣ</returns>
    IList<CustomizeContentInfo> FindAll(string whereClause, int length);
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
    IList<CustomizeContentInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
    #endregion

    #region ����:IsExist(string id)
    /// <summary>��ѯ�Ƿ������صļ�¼</summary>
    /// <param name="param">CustomizeContentInfo ʵ����ϸ��Ϣ</param>
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
