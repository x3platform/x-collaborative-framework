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

  public class ParseTemplateVirtualPath : Directive
  {
    public class VirtualPathTemplate : Template
    {
      public void Load(VelocityEngine engine, string virtualPath)
      {
        this.runtimeServices = engine.RuntimeServices;

        this.Name = virtualPath;

        this.Process();
      }

      protected override Stream ObtainStream()
      {
        return FileHelper.ToStream(VirtualPathHelper.GetPhysicalPath(name));
      }
    }

    /// <summary>
    /// Return name of this directive.
    /// </summary>
    public override String Name
    {
      get { return "parseTemplateVirtualPath"; }
      set { throw new NotSupportedException(); }
    }

    /// <summary> Return type of this directive. </summary>
    public override DirectiveType Type
    {
      get { return DirectiveType.LINE; }
    }

    /// <summary>
    /// iterates through the argument list and renders every
    /// argument that is appropriate.  Any non appropriate
    /// arguments are logged, but render() continues.
    /// </summary>
    public override bool Render(IInternalContextAdapter context, TextWriter writer, INode node)
    {
      // did we get an argument?
      if (!AssertArgument(node))
      {
        return false;
      }

      // does it have a value?  If you have a null reference, then no.
      Object value;
      if (!AssertNodeHasValue(node, context, out value))
      {
        return false;
      }

      // get the path
      String arg = value.ToString();

      AssertTemplateStack(context);

      Resource current = context.CurrentResource;

      // get the resource, and assume that we use the encoding of the current template
      // the 'current resource' can be null if we are processing a stream....
      String encoding;

      if (current == null)
      {
        encoding = (String)runtimeServices.GetProperty(RuntimeConstants.INPUT_ENCODING);
      }
      else
      {
        encoding = current.Encoding;
      }

      Template template = VelocityManager.Instance.GetTemplateByVirtualPath(arg);

      try
      {
        context.PushCurrentTemplateName(arg);
        ((SimpleNode)template.Data).Render(context, writer);
      }
      catch (Exception)
      {
        // if it's a MIE, it came from the render.... throw it...
        // if (e is MethodInvocationException)
        throw;

        // runtimeServices.Error("Exception rendering #parse( " + arg + " )  : " + e);
        // result = false;
      }
      finally
      {
        context.PopCurrentTemplateName();
      }

      return true;
    }

    private bool AssertArgument(INode node)
    {
      bool result = true;
      if (node.GetChild(0) == null)
      {
        runtimeServices.Error("#parse() error :  null argument");
        result = false;
      }
      return result;
    }

    private bool AssertNodeHasValue(INode node, IInternalContextAdapter context, out Object value)
    {
      bool result = true;

      value = node.GetChild(0).Value(context);

      if (value == null)
      {
        runtimeServices.Error("#parse() error :  null argument");
        result = false;
      }
      return result;
    }

    /// <summary>
    /// See if we have exceeded the configured depth.
    /// If it isn't configured, put a stop at 20 just in case.
    /// </summary>
    private bool AssertTemplateStack(IInternalContextAdapter context)
    {
      bool result = true;
      Object[] templateStack = context.TemplateNameStack;

      if (templateStack.Length >= runtimeServices.GetInt(RuntimeConstants.PARSE_DIRECTIVE_MAXDEPTH, 20))
      {
        StringBuilder path = new StringBuilder();

        for (int i = 0; i < templateStack.Length; ++i)
        {
          path.AppendFormat(" > {0}", (object[])templateStack[i]);
        }

        runtimeServices.Error(string.Format("Max recursion depth reached ({0}) File stack:{1}", templateStack.Length, path));
        result = false;
      }
      return result;
    }


    private Template GetTemplate(String arg, String encoding, IInternalContextAdapter context)
    {
      Template result;

      try
      {
        result = runtimeServices.GetTemplate(arg, encoding);
      }
      catch (ResourceNotFoundException)
      {
        // the arg wasn't found.  Note it and throw
        runtimeServices.Error(
            string.Format("#parse(): cannot find template '{0}', called from template {1} at ({2}, {3})", arg,
                          context.CurrentTemplateName, Line, Column));
        throw;
      }
      catch (ParseErrorException)
      {
        // the arg was found, but didn't parse - syntax error
        // note it and throw
        runtimeServices.Error(
            string.Format("#parse(): syntax error in #parse()-ed template '{0}', called from template {1} at ({2}, {3})", arg,
                          context.CurrentTemplateName, Line, Column));

        throw;
      }
      catch (Exception e)
      {
        runtimeServices.Error(string.Format("#parse() : arg = {0}.  Exception : {1}", arg, e));
        result = null;
      }
      return result;
    }

    private bool RenderTemplate(Template template, String arg, TextWriter writer, IInternalContextAdapter context)
    {
      bool result = true;
      try
      {
        context.PushCurrentTemplateName(arg);
        ((SimpleNode)template.Data).Render(context, writer);
      }
      catch (Exception)
      {
        // if it's a MIE, it came from the render.... throw it...
        // if (e is MethodInvocationException)
        throw;

        // runtimeServices.Error("Exception rendering #parse( " + arg + " )  : " + e);
        // result = false;
      }
      finally
      {
        context.PopCurrentTemplateName();
      }

      return result;
    }
  }
}