namespace X3Platform.Velocity.Runtime.Directive
{
  using System;
  using System.IO;
  using System.Text;
  using Context;
  using X3Platform.Velocity.Exception;
  using X3Platform.Velocity.Runtime.Parser.Node;
  using Resource;
  using X3Platform.Web;
  using X3Platform.Util;
  using System.Resources;
  using X3Platform.Velocity.App;
  using X3Platform.Globalization;

  public class I18nLiteral : Directive
  {
    /// <summary>
    /// Return name of this directive.
    /// </summary>
    public override String Name
    {
      get { return "i18n"; }
      set { throw new NotSupportedException(); }
    }

    /// <summary> Return type of this directive. </summary>
    public override DirectiveType Type
    {
      get { return DirectiveType.LINE; }
    }

    /// <summary></summary>
    public override bool Render(IInternalContextAdapter context, TextWriter writer, INode node)
    {
      string text = node.GetChild(0).Literal;

      string[] args = text.Substring(1, text.Length - 2).Split(',');
      string[] keys = args[0].Split('.');

      StringCase stringCase = StringCase.Default;

      if (args.Length == 2)
      {
        stringCase = (StringCase)Enum.Parse(typeof( StringCase), args[1]);
      }

      if (keys[0] == "Strings")
      {
        if (keys.Length == 2)
        {
          writer.Write(I18n.Strings[keys[1], stringCase]);
        }
        else if (keys.Length == 3)
        {
          writer.Write(I18n.Strings[keys[1], keys[2], stringCase]);
        }
      }
      else if (keys[0] == "Translates")
      {
        if (keys.Length == 2)
        {
          writer.Write(I18n.Translates[keys[1], stringCase]);
        }
        else if (keys.Length == 3)
        {
          writer.Write(I18n.Translates[keys[1], keys[2], stringCase]);
        }
      }

      return true;
    }

  }
}