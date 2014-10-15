using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Velocity.App;
using X3Platform.Velocity.Runtime;

namespace X3Platform.Velocity.TestSuite
{
    /// <summary></summary>
    [TestClass]
    public class VelocityManagerTestSuite
    {
        #region 其他测试属性
        //
        // 您可以在编写测试时使用下列其他属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前使用 TestInitialize 运行代码 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在运行每个测试之后使用 TestCleanup 运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetTemplateByString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("#foreach($u in $ListUsers)\r\n" +
               "#beforeall\r\n" +
               "<table border=\"0\" cellpadding=\"10\" cellspacing=\"10\">" +
               "<tr><td>Name</td><td>Sex</td><td>City</td></tr>" +
               "#each\r\n" +
               "<tr>" +
               "<td>$u.Name</td>" +
               "<td>$u.Sex</td>" +
               "<td>$u.City</td>" +
               "</tr>" +
               "#afterall\r\n" +
               "</table>" +
               "#nodata\r\n" +
               "暂无用户资料\r\n" +
               "#end");
            IList<UserInfo> listUsers = new List<UserInfo>();

            UserInfo objUser = new UserInfo();
            objUser.Name = "测试员一";
            objUser.Sex = "男";
            objUser.City = "深圳";
            listUsers.Add(objUser);

            objUser = new UserInfo();
            objUser.Name = "测试员二";
            objUser.Sex = "女";
            objUser.City = "北京";
            listUsers.Add(objUser);

            objUser = new UserInfo();
            objUser.Name = "测试员三";
            objUser.Sex = "男";
            objUser.City = "非洲";
            listUsers.Add(objUser);

            IList<string> listAssembly = new List<string>();

            // 添加程序集名称
            listAssembly.Add("LibTest");

            //VelocityEngine velocityEngine = new VelocityEngine();

            //velocityEngine.Init();
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            VelocityContext velocityContext = new VelocityContext();

            velocityContext.Put("PageTitle", "资源模板例子");
            velocityContext.Put("ListUsers", listUsers);

            //Template velocityTemplate = velocityEngine.GetTemplate("Default.htm");

            StringWriter stringWriter = new StringWriter();

            stringWriter.GetStringBuilder().ToString();
        }

        [TestMethod]
        public void TestGetTemplateByFile()
        {
            IList<UserInfo> listUsers = new List<UserInfo>();

            UserInfo objUser = new UserInfo();
            objUser.Name = "测试员一";
            objUser.Sex = "男";
            objUser.City = "深圳";
            listUsers.Add(objUser);

            objUser = new UserInfo();
            objUser.Name = "测试员二";
            objUser.Sex = "女";
            objUser.City = "北京";
            listUsers.Add(objUser);

            objUser = new UserInfo();
            objUser.Name = "测试员三";
            objUser.Sex = "男";
            objUser.City = "非洲";
            listUsers.Add(objUser);

            VelocityContext context = new VelocityContext();

            context.Put("PageTitle", "资源模板例子");
            context.Put("ListUsers", listUsers);

            string result = VelocityManager.Instance.Merge(context, "default.vm");

            Assert.IsNotNull(result);

            result = VelocityManager.Instance.Merge(context, "global\\default.vm");

            Assert.IsNotNull(result);
        }
    }

    public class UserInfo
    {
        private string _Name;
        private string _Sex;
        private string _City;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
    }
}
