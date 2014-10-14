// =============================================================================
// 模块名称 : 权限配置
// =============================================================================
//
// 项目名称		:权限配置
//
// 功能描述		:负责项目的权限配置
//
// 负责人       :阮郁
//
// 日期         :2010-01-01
//
// =============================================================================

登录时
protected void btnLogin_Click(object sender, System.EventArgs e)
{
    AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
    
    if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
    {
        try
        {
            if (am.AuthenticateUser(this.txtUsername.Text, this.txtPassword.Text, this.chkPersistLogin.Checked))
            {
                //通过认证
                Context.Response.Redirect(Context.Request.RawUrl);
            }
            else
            {
                //认证失败
            }
        }
        catch (Exception ex)
        {
        }
    }
}

退出登录用
protected void btnLogout_Click(object sender, System.EventArgs e)
{
    AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
    am.Logout();
    Context.Response.Redirect(Context.Request.RawUrl);
}

这样就实现了身份认证功能，然后可以方便的实现权限认证。

在User类中实现相应的权限逻辑 如: 表示当前用户是否有权限浏览指定的节点
public bool CanView(Node node)
{
    foreach (Permission p in node.NodePermissions)
    {
        if (p.ViewAllowed && IsInRole(p.Role))
        {
            return true;
        }
    }
    return false;
}

在Page代码中嵌入验证代码即可
User CuyahogaUser =  this.User.Identity as User;
if(CuyahogaUser.CanView())
{
}