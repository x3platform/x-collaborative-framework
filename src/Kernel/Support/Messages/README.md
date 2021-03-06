## =============================================================================
## 模块名称 : 消息队列管理
## =============================================================================
##
## 项目名称		:消息队列管理
##
## 功能描述		:负责提供系统消息队列支持.
##
## 负责人       :阮郁
##
## 日期         :2010-01-01
##
## =============================================================================

Windows Server 2008 安装和卸载消息队列及队列权限设置

安装消息队列的步骤

    如果最近安装了 Windows Server 2008，并且显示了“初始配置任务”界面，则在“自定义此服务器”下单击“添加功能”。然后跳到步骤 3。
    
	如果没有显示“初始配置任务”界面，且服务器管理器没有运行，则依次单击「开始」、“管理工具”和“服务器管理器”。 
	(如果出现“用户帐户控制”对话框，请确认所显示的是您要执行的操作，然后单击“继续”。)

    然后，在“服务器管理器”的“功能摘要”下，单击“添加功能”。
    在“添加功能向导”中，依次展开 MSMQ 和“MSMQ 服务”，然后选中要安装的消息队列功能所对应的复选框。
    单击“下一步”，然后单击“安装”。
    如果系统提示您重新启动计算机，则单击“确定”以完成安装。

卸载消息队列的步骤

    如果服务器管理器尚未打开，请依次单击「开始」、“管理工具”和“服务器管理器”。 (如果出现“用户帐户控制”对话框，请确认所显示的是您要执行的操作，然后单击“继续”。)
    在“服务器管理器”的“功能摘要”下，单击“删除功能”。
    在“删除功能向导”中，依次展开 MSMQ 和“MSMQ 服务”，然后清除要卸载的消息队列功能所对应的复选框。

    在此向导中，可以通过清除（不是选中）复选框删除某个功能。
    单击“下一步”，然后单击“删除”。
    收到提示时，请单击“确定”以重新启动计算机。

队列权限设置：

使用“Active Directory 用户和计算机”设置消息队列计算机和队列对象的权限的步骤

    单击“开始”，依次指向“所有程序”、“管理工具”，然后单击“Active Directory 用户和计算机”。

    在“查看”菜单上，单击“作为容器的用户、组和计算机”，然后单击“高级功能”。

    执行下列操作之一：
        若要授予计算机（msmq 对象）的消息队列特定的权限，请在控制台树中，右键单击 msmq（控制台树的位置为：Active Directory 用户和计算机/YourDomain/YourOrganizationalUnit（如计算机或域控制器）/YourComputer/msmq）。
        若要向队列授予消息队列特定的权限，请右键单击适用的队列。（控制台树的位置为：Active Directory 用户和计算机\ YourDomain\ YourOrganizationalUnit（如计算机或域控制器）\ YourComputer\ msmq\ YourQueueFolder（用于专用队列的“专用队列”）\ YourQueue）。

    单击“属性”。

    在“安全”页上，根据需要设置步骤 3 中所指定的对象的权限：
        若要将此对象的权限授予“组或用户名”下出现的组或用户，请选择适用的组或用户，然后在“GroupOrUser 的权限”中，选中适用的权限名称后面的“允许”列中的复选框。
        若要拒绝此对象的组或用户权限，请在“组或用户名”中，选择适用的组或用户，然后在“GroupOrUser 的权限”中，选中适用的权限名称后面的“拒绝”列中的复选框。
        若要添加新的组或用户进行访问，请单击“添加”。在“选择用户、计算机或组”对话框中，单击“对象类型”，根据需要选中“组”和/或“用户”复选框，清除其余的复选框，然后单击“确定”。在“输入要选择的对象名称”中，键入一个组或用户的名称或键入多个组或用户的名称（以分号分隔），然后单击“确定”。或者，单击“高级”搜索组或用户，输入相应的参数，单击“立即查找”，选择组或用户，单击“确定”，然后再次单击“确定”。然后，选择刚添加的组或用户，并选中适用的复选框。

使用“计算机管理”设置队列对象的权限的步骤

单击“开始”，指向“运行”，键入 compmgmt.msc，然后按 Enter 以显示“计算机管理 MMC”控制台。

在控制台树中，右键单击适用的队列。

位置：
    计算机管理/服务和应用程序/消息队列/YourQueueFolder（如“公用队列”或“专用队列”）/YourQueue

单击“属性”。

在“安全”页上，根据需要设置队列的权限：
    若要将此对象的权限授予“组或用户名”下出现的组或用户，请选择适用的组或用户，然后在“GroupOrUser 的权限”中，选中适用的权限名称后面的“允许”列中的复选框。
    若要拒绝此对象的组或用户权限，请在“组或用户名”中，选择适用的组或用户，然后在“GroupOrUser 的权限”中，选中适用的权限名称后面的“拒绝”列中的复选框。
    若要添加新的组或用户进行访问，请单击“添加”。在“选择用户、计算机或组”对话框中，单击“对象类型”，根据需要选中“组”和/或“用户”复选框，清除其余的复选框，然后单击“确定”。在“输入要选择的对象名称”中，键入一个组或用户的名称或键入多个组或用户的名称（以分号分隔），然后单击“确定”。或者，单击“高级”搜索组或用户，输入相应的参数，单击“立即查找”，选择组或用户，单击“确定”，然后再次单击“确定”。然后，选择刚添加的组或用户，并选中适用的复选框。
