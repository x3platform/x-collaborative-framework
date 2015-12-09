namespace X3Platform.TemplateContent.IBLL
{
    #region Using Libraries
    using System.Collections.Generic;
    using X3Platform.Data;
    using X3Platform.Spring;
    using X3Platform.TemplateContent.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.TemplateContent.IBLL.ITemplateContentService")]
    public interface ITemplateContentService
    {
        #region ����:this[string index]
        /// <summary>����</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TemplateContentInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ�� TemplateContentInfo ʵ������ϸ��Ϣ</returns>
        TemplateContentInfo FindOneByName(string name);
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