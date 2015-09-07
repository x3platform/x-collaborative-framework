namespace X3Platform.DigitalNumber.Model
{
  #region Using Libraries
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Resources;
  using System.Text;
  #endregion

  /// <summary>流水号信息</summary>
  public class DigitalNumberInfo
  {
    #region 构造函数:DigitalNumberInfo()
    /// <summary>默认构造函数</summary>
    public DigitalNumberInfo() { }
    #endregion

    #region 属性:Name
    private string m_Name;

    /// <summary></summary>
    public string Name
    {
      get { return m_Name; }
      set { m_Name = value; }
    }
    #endregion

    #region 属性:Expression
    private string m_Expression;

    /// <summary></summary>
    public string Expression
    {
      get { return m_Expression; }
      set { m_Expression = value; }
    }
    #endregion

    #region 属性:Seed
    private int m_Seed;

    /// <summary></summary>
    public int Seed
    {
      get { return m_Seed; }
      set { m_Seed = value; }
    }
    #endregion

    #region 属性:UpdateDate
    private DateTime m_UpdateDate;

    /// <summary></summary>
    public DateTime UpdateDate
    {
      get { return m_UpdateDate; }
      set { m_UpdateDate = value; }
    }
    #endregion

    #region 属性:CreateDate
    private DateTime m_CreateDate;

    /// <summary></summary>
    public DateTime CreateDate
    {
      get { return m_CreateDate; }
      set { m_CreateDate = value; }
    }
    #endregion

  }
}
