// =============================================================================
// ģ������ : Ȩ������
// =============================================================================
//
// ��Ŀ����		:Ȩ������
//
// ��������		:������Ŀ��Ȩ������
//
// ������       :����
//
// ����         :2010-01-01
//
// =============================================================================

��¼ʱ
protected void btnLogin_Click(object sender, System.EventArgs e)
{
    AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
    
    if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
    {
        try
        {
            if (am.AuthenticateUser(this.txtUsername.Text, this.txtPassword.Text, this.chkPersistLogin.Checked))
            {
                //ͨ����֤
                Context.Response.Redirect(Context.Request.RawUrl);
            }
            else
            {
                //��֤ʧ��
            }
        }
        catch (Exception ex)
        {
        }
    }
}

�˳���¼��
protected void btnLogout_Click(object sender, System.EventArgs e)
{
    AuthenticationModule am = (AuthenticationModule)Context.ApplicationInstance.Modules["AuthenticationModule"];
    am.Logout();
    Context.Response.Redirect(Context.Request.RawUrl);
}

������ʵ���������֤���ܣ�Ȼ����Է����ʵ��Ȩ����֤��

��User����ʵ����Ӧ��Ȩ���߼� ��: ��ʾ��ǰ�û��Ƿ���Ȩ�����ָ���Ľڵ�
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

��Page������Ƕ����֤���뼴��
User CuyahogaUser =  this.User.Identity as User;
if(CuyahogaUser.CanView())
{
}