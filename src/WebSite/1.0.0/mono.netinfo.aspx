<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="utf-8" %>

<script language="C#" runat="server">

    // ========================================
    // 程序名称：.Net空间信息 双向探针
    // Version:1.0
    // Written by RuanYu
    // Copyright (C) 2010 RuanYu All rights reserved.
    // Email:ruanyu@live.com
    // 本程序代码完全免费，您可以任意复制，修改和传播。但请保留以上信息谢谢合作。
    // ---------------------------------------
    // 修改请注明由本程序修改而来，谢谢合作。
    // 注意：本程序谢绝用于任何商业领域，修改版本不得以任何名义收费。
    // 虚拟主机商使用本程序请先联系 ruanyu@live.com。我将把你的链接加入列表，并第一时间提供最新版本。
    // ========================================

    public void Page_Load(Object sender, EventArgs e)
    {
        // 设置过期时间
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (!Page.IsPostBack)
        {
            // 取得页面执行开始时间
            DateTime stime = DateTime.Now;

            // 取得服务器相关信息
            servername.Text = Server.MachineName;
            serverip.Text = Request.ServerVariables["LOCAL_ADDR"];
            server_name.Text = Request.ServerVariables["SERVER_NAME"];

            // 以下就是取值不准的地方，因为用了HTTP_USER_AGENT当做服务器信息。
            // 1.0 final 使用Environment类属性，彻底解决了这一问题
            // char[] de = {';'};
            // string allhttp=Request.ServerVariables["HTTP_USER_AGENT"].ToString();
            // string[] myFilename = allhttp.Split(de);
            // servernet.Text=myFilename[myFilename.Length-1].Replace(")"," ");
            int build, major, minor, revision;
            build = Environment.Version.Build;
            major = Environment.Version.Major;
            minor = Environment.Version.Minor;
            revision = Environment.Version.Revision;
            servernet.Text = ".NET CLR  " + major + "." + minor + "." + build + "." + revision;
            serverms.Text = Environment.OSVersion.ToString();

            //服务器端浏览器版本暂时不知道怎么取得，原有不准，故删除
            //1.0 final 修改
            //serverie.Text=myFilename[1];

            serversoft.Text = Request.ServerVariables["SERVER_SOFTWARE"];
            serverport.Text = Request.ServerVariables["SERVER_PORT"];
            serverout.Text = Server.ScriptTimeout.ToString();
            // 语言应该是浏览者信息，1.0 final 修改
            cl.Text = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            servertime.Text = DateTime.Now.ToString();
            serverppath.Text = Request.ServerVariables["APPL_PHYSICAL_PATH"];
            servernpath.Text = Request.ServerVariables["PATH_TRANSLATED"];
            serverhttps.Text = Request.ServerVariables["HTTPS"];

            if (IsExistComponent("ADODB.RecordSet"))
            {
                serveraccess.Text = "支持";
            }
            else
            {
                serveraccess.Text = "不支持";
            }

            if (IsExistComponent("Scripting.FileSystemObject"))
            {
                serverfso.Text = "支持";
            }
            else { serverfso.Text = "不支持"; }

            if (IsExistComponent("CDONTS.NewMail"))
            {
                servercdonts.Text = "支持";
            }
            else
            {
                servercdonts.Text = "不支持";
            }
            servers.Text = Session.Contents.Count.ToString();
            servera.Text = Application.Contents.Count.ToString();

            // 0.1版添加的组件验证，原有组件并未转移过来，请原谅。
            if (IsExistComponent("JMail.SmtpMail"))
            {
                jmail.Text = "支持";
            }
            else { jmail.Text = "不支持"; }

            if (IsExistComponent("Persits.MailSender"))
            {
                aspemail.Text = "支持";
            }
            else { aspemail.Text = "不支持"; }

            if (IsExistComponent("Geocel.Mailer"))
            {
                geocel.Text = "支持";
            }
            else { geocel.Text = "不支持"; }

            if (IsExistComponent("SmtpMail.SmtpMail.1"))
            {
                smtpmail.Text = "支持";
            }
            else { smtpmail.Text = "不支持"; }

            if (IsExistComponent("Persits.Upload.1"))
            {
                aspup.Text = "支持";
            }
            else { aspup.Text = "不支持"; }

            if (IsExistComponent("aspcn.Upload"))
            {
                aspcnup.Text = "支持";
            }
            else { aspcnup.Text = "不支持"; }

            if (IsExistComponent("LyfUpload.UploadFile"))
            {
                lyfup.Text = "支持";
            }
            else { lyfup.Text = "不支持"; }

            if (IsExistComponent("SoftArtisans.FileManager"))
            {
                soft.Text = "支持";
            }
            else { soft.Text = "不支持"; }

            if (IsExistComponent("w3.upload"))
            {
                dimac.Text = "支持";
            }
            else { dimac.Text = "不支持"; }

            if (IsExistComponent("W3Image.Image"))
            {
                dimacimage.Text = "支持";
            }
            else { dimacimage.Text = "不支持"; }

            //取得用户浏览器信息
            HttpBrowserCapabilities bc = Request.Browser;
            ie.Text = bc.Browser.ToString();
            cookies.Text = bc.Cookies.ToString();
            frames.Text = bc.Frames.ToString();
            javaa.Text = bc.JavaApplets.ToString();
            javas.Text = bc.JavaScript.ToString();
            ms.Text = bc.Platform.ToString();
            vbs.Text = bc.VBScript.ToString();
            vi.Text = bc.Version.ToString();

            //取得浏览者ip地址,1.0 final 加入
            cip.Text = Request.ServerVariables["REMOTE_ADDR"];

            //取得页面执行结束时间
            DateTime etime = DateTime.Now;


            // 计算页面执行时间
            runtime.Text = ((etime - stime).TotalMilliseconds).ToString();
        }
    }

    // 检测支持组件
    bool IsExistComponent(string componentName)
    {
        try
        {
            object component = Server.CreateObject(componentName);

            return true;
        }
        catch
        {
            return false;
        }
    }

    //100万次循环测试，由0.1sn bulid 021203开始加入

    public void turn_chk(Object Sender, EventArgs e)
    {
        DateTime beginTime = DateTime.Now;

        int sum = 0;

        for (int i = 1; i <= 10000000; i++)
        {
            sum = sum + i;
        }

        DateTime endTime = DateTime.Now;

        l1000.Text = ((endTime - beginTime).TotalMilliseconds).ToString() + "毫秒";
    }

    // 自定义组件检测0.1版加入

    public void chkzujian(Object Sender, EventArgs e)
    {
        string obj = zujian.Text;

        if (IsExistComponent(obj))
        {
            l001.Text = "检测结果：支持组件" + obj;
        }
        else
        {
            l001.Text = "检测结果：不支持组件" + obj;
        }
    }
</script>
<!DOCTYPE HTML>
<html>
<head>
<title>.Net 空间信息</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<style type="text/css">
body *{ font-size:12px; font-family: 微软雅黑;}
.table-group-title { font-size:14px; background-color:#f5f5f5; padding:6px 0 4px 10px;}
.table-group-item-text { text-align:right; padding:6px 10px 4px 10px;}
.table-group-item-input { text-align:left; padding:6px 10px 4px 10px;}
</style>
</head>
<body>
<div align="center">
<form id="Form1" runat="server">
<table width="760" height="80" border="1" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-color: Black; border-width: 1px; border-style: solid; font-family: Verdana; border-collapse: collapse;">
<tr>
<td width="260">
<div align="center">
<font>.Net Framework</font><br />
<font style="font-size:36px; letter-spacing:4px;" >双向探针</font><br />
</div>
</td>
<td>
<div align="center">
<table width="468" height="60" border="0" cellpadding="0" cellspacing="0" style="border-color: Black; border-width: 1px; border-style: solid; font-family: Verdana; border-collapse: collapse;">
<tr>
<td valign="top">
<!-- 广告位 -->
</td>
</tr>
</table>
</div>
</td>
</tr>
</table>

<table width="760" border="1" cellpadding="1" cellspacing="1" style="border-color: Black; border-width: 1px; border-style: solid; font-family: Verdana; border-collapse: collapse;">
<tr>
<td class="table-group-title" colspan="4" >.NET 服务器信息</td>
</tr>
<tr>
<td class="table-group-item-text" style="width:20%" >服务器名称</td>
<td class="table-group-item-input" style="width:30%" ><asp:Label ID="servername" runat="server" /></td>
<td class="table-group-item-text" style="width:20%" >服务器操作系统</td>
<td class="table-group-item-input" style="width:30%" ><asp:Label ID="serverms" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >服务器IP地址</td>
<td class="table-group-item-input" ><asp:Label ID="serverip" runat="server" /></td>
<td class="table-group-item-text" >服务器域名</td>
<td class="table-group-item-input"><asp:Label ID="server_name" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >服务器IIS版本</td>
<td class="table-group-item-input" ><asp:Label ID="serversoft" runat="server" /></td>
<td class="table-group-item-text" >.NET解释引擎版本</td>
<td class="table-group-item-input" ><asp:Label ID="servernet" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >HTTPS</td>
<td class="table-group-item-input" ><asp:Label ID="serverhttps" runat="server" /></td>
<td class="table-group-item-text" >HTTP访问端口</td>
<td class="table-group-item-input" ><asp:Label ID="serverport" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >服务端脚本执行超时</td>
<td class="table-group-item-input" ><asp:Label ID="serverout" runat="server" /> 秒</td>
<td class="table-group-item-text" >服务器现在时间</td>
<td class="table-group-item-input"><asp:Label ID="servertime" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >虚拟目录绝对路径</td>
<td class="table-group-item-input" colspan="3" ><asp:Label ID="serverppath" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >执行文件绝对路径</td>
<td class="table-group-item-input" colspan="3" ><asp:Label ID="servernpath" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >虚拟目录Session总数</td>
<td class="table-group-item-input" ><asp:Label ID="servers" runat="server" /></td>
<td class="table-group-item-text" >虚拟目录Application总数</td>
<td class="table-group-item-input"><asp:Label ID="servera" runat="server" /></td>
</tr>
<tr>
<td class="table-group-title" colspan="4" >常见组件支持情况</td>
</tr>
<tr>
<tr>
<td class="table-group-item-text" >Access数据库</td>
<td class="table-group-item-input"><asp:Label ID="serveraccess" runat="server" /></td>
<td class="table-group-item-text" >FSO</td>
<td class="table-group-item-input"><asp:Label ID="serverfso" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >CDONTS邮件发送</td>
<td class="table-group-item-input" ><asp:Label ID="servercdonts" runat="server" /></td>
<td class="table-group-item-text" >JMail邮件收发</td>
<td class="table-group-item-input" ><asp:Label ID="jmail" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >ASPemail发信</td>
<td class="table-group-item-input" ><asp:Label ID="aspemail" runat="server" /></td>
<td class="table-group-item-text" >Geocel发信</td>
<td class="table-group-item-input" ><asp:Label ID="geocel" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >SmtpMail发信</td>
<td class="table-group-item-input" ><asp:Label ID="smtpmail" runat="server" /></td>
<td class="table-group-item-text" >ASPUpload文件上传</td>
<td class="table-group-item-input"><asp:Label ID="aspup" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >ASPCN文件上传</td>
<td class="table-group-item-input" ><asp:Label ID="aspcnup" runat="server" /></td>
<td class="table-group-item-text" >刘云峰的文件上传组件</td>
<td class="table-group-item-input" ><asp:Label ID="lyfup" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >SoftArtisans文件管理</td>
<td class="table-group-item-input"><asp:Label ID="soft" runat="server" /></td>
<td class="table-group-item-text" >Dimac文件上传</td>
<td class="table-group-item-input"><asp:Label ID="dimac" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >Dimac的图像读写组件</td>
<td class="table-group-item-input"><asp:Label ID="dimacimage" runat="server" /></td>
<td class="table-group-item-text" >自定义组件查询</td>
<td class="table-group-item-input">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr>
<td width="72%">
<font size="2">
<asp:TextBox ID="zujian" Rows="1" runat="server" TextMode="SingleLine" />
</font>
</td>
<td width="28%">
<font size="2">
<asp:Button ID="ckzu" runat="server" Text="检测" OnClick="chkzujian" />
</font>
</td>
</tr>
</table>
</td>
</tr>
<tr>
<td class="table-group-item-input" colspan="4">
<font >请正确输入你要检测的组件的ProgId或ClassId。</font><br />
<font color="#FF0000"><asp:Label ID="l001" runat="server" /></font>
</td>
</tr>
<tr>
<td class="table-group-title" colspan="4" >客户端信息</td>
</tr>
<tr>
<td class="table-group-item-text" >浏览者IP地址</td>
<td class="table-group-item-input" ><asp:Label ID="cip" runat="server" /></td>
<td class="table-group-item-text" >浏览者操作系统</td>
<td class="table-group-item-input"><asp:Label ID="ms" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >浏览器</td>
<td class="table-group-item-input"><asp:Label ID="ie" runat="server" /></td>
<td class="table-group-item-text" >浏览器版本</td>
<td class="table-group-item-input"><asp:Label ID="vi" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >JavaScript</td>
<td class="table-group-item-input" ><asp:Label ID="javas" runat="server" /></td>
<td class="table-group-item-text" >VBScript</td>
<td class="table-group-item-input"><asp:Label ID="vbs" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >JavaApplets</td>
<td class="table-group-item-input" ><asp:Label ID="javaa" runat="server" /></td>
<td class="table-group-item-text" >Cookies</td>
<td class="table-group-item-input"><asp:Label ID="cookies" runat="server" /></td>
</tr>
<tr>
<td class="table-group-item-text" >语言</td>
<td class="table-group-item-input" ><asp:Label ID="cl" runat="server" /></td>
<td class="table-group-item-text" >Frames（分栏）</td>
<td class="table-group-item-input" ><asp:Label ID="frames" runat="server" /></td>
</tr>
<tr>
<td class="table-group-title" colspan="4" >执行效率相关情况</td>
</tr>
<tr>
<td class="table-group-item-text" >本页执行时间</td>
<td class="table-group-item-input" ><asp:Label ID="runtime" runat="server" />毫秒</td>
<td class="table-group-item-text" >1000万次加法循环测试</td>
<td class="table-group-item-input" >
<asp:Button ID="for1000" runat="server" OnClick="turn_chk" Text="测试" />
<asp:Label ID="l1000" runat="server" />
</td>
</tr>
</table>
</form>
</div>
</body>
</html>
