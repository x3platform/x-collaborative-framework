using System;
using System.Text;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Diagnostics;
using System.Data.Common;
using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elane.X.ActiveDirectory.Interop.TestSuite
{
    /// <summary>
    /// ActiveDirectorySynchronizerTestSuite 的摘要说明
    /// </summary>
    [TestClass]
    public class UserProviderTestSuite
    {
        //IActiveDirectoryUtilityProvider ActiveDirectoryManagement.Instance = ActiveDirectoryUtilityFactory.GetActiveDirectoryUtilityDefaultProvider();

        //Database db = DatabaseFactory.CreateDatabase("LH_BDM");

        public UserProviderTestSuite()
        {

        }

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
        public void TestFindAll()
        {

            StringBuilder outString = new StringBuilder();

            StringBuilder logging = new StringBuilder();

            string[] fullNames = new string[]{
                "组织用户"
            };

            DirectoryEntry param = null;

            int count = 0;

            int maxUserNumber = 20000;

            try
            {
                // 加这个x的循环, 是为了把重名的员工也导过来...
                for (int i = 0; i < fullNames.Length; i++)
                {
                    outString.AppendLine(fullNames[i]);

                    SearchResultCollection list = ActiveDirectoryManagement.Instance.User.FindAll(string.Format("OU={0}", fullNames[i]));

                    foreach (SearchResult item in list)
                    {
                        param = item.GetDirectoryEntry();

                        outString.AppendLine(string.Format("{0},{1}", param.Properties["sAMAccountName"].Value, param.Name));
                    }
                }

                File.AppendAllText("e:\\user.list.text", outString.ToString());

                //    if (!MemberShip.MemberShipEngine.Instance.User.IsExist(param.Properties["sAMAccountName"].Value.ToString()))
                //    {
                //        logging.AppendLine(string.Format("{0} #### 不存在帐号{1} {2}", fullNames[i], param.Name, param.Properties["sAMAccountName"].Value));

                //        try
                //        {
                //            Guid g = Guid.NewGuid();

                //            DbCommand dbCommand = db.GetSqlStringCommand(string.Format("INSTER INTO USERS ([UserId],[LoginId],[UserName]) VALUES ('{0}','{1}','{2}'); INSERT INTO UserOrg VALUES ('{0}', '00000000-0000-0000-0000-000000000001','2008-08-08');", g, param.Properties["sAMAccountName"].Value, param.Name));

                //            db.ExecuteNonQuery(dbCommand);
                //        }
                //        catch (Exception e)
                //        {
                //            Trace.WriteLine(string.Format("帐号{0}添加失败.\r\n{1}", param.Name, e.Message));
                //        }

                //        outString.AppendLine(string.Format("####不存在帐号{0} {1}", param.Name, param.Properties["sAMAccountName"].Value));
                //    }
                //    else
                //    {
                //        try
                //        {
                //            DbCommand dbCommand = db.GetSqlStringCommand(string.Format("UPDATE USERS SET [UserName] = '{1}' Where LoginId= '{0}'", param.Properties["sAMAccountName"].Value, param.Name));

                //            db.ExecuteNonQuery(dbCommand);
                //        }
                //        catch (Exception e)
                //        {
                //            Trace.WriteLine(string.Format("帐号{0}添加失败.\r\n{1}", param.Name, e.Message));
                //        }

                //        outString.AppendLine(string.Format("===已存在帐号{0} {1}", param.Name, param.Properties["sAMAccountName"].Value));
                //    }

                //    ActiveDirectoryManagement.Instance.Group.AddRelation(param.Properties["sAMAccountName"].Value.ToString(), "user", "所有人");
                //}

                //if (count > maxUserNumber)
                //    break;

                outString.AppendLine("条数:" + count);

                outString.Insert(0, logging.ToString());

                Trace.WriteLine(outString.ToString());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        [TestMethod]
        public void TestAdd()
        {
            ActiveDirectoryManagement.Instance.User.Add("a100", "pass@word1", "万琳1", "123456", "we@google.com");
        }

        [TestMethod]
        public void TestRemove()
        {
            string loginName = "a100";

            ActiveDirectoryManagement.Instance.User.Remove(loginName);
        }

        [TestMethod]
        public void TestMoveTo()
        {
            string loginName = "a100";

            ActiveDirectoryManagement.Instance.User.MoveTo("OU=组织结构", loginName);
        }

        [TestMethod]
        [DeploymentItem("config/Longfor.ActiveDirectoryUtility.config", "config/")]
        public void TestCheckPasswordStrategy()
        {
            try
            {
                ActiveDirectoryManagement.Instance.User.CheckPasswordStrategy("123456");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        [TestMethod]
        [DeploymentItem("config/Longfor.ActiveDirectoryUtility.config", "config/")]
        public void TestSetPassword()
        {
            try
            {
                string loginName = "mossmanager";

                ActiveDirectoryManagement.Instance.User.SetPassword(loginName, "123456");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        [TestMethod]
        [DeploymentItem("config/Longfor.ActiveDirectoryUtility.config", "config/")]
        public void TestChangePassword()
        {
            string loginName = "mossmanager";

            ActiveDirectoryManagement.Instance.User.ChangePassword(loginName, "123456", "123456");
        }

        [TestMethod]
        [DeploymentItem("config/Longfor.ActiveDirectoryUtility.config", "config/")]
        public void TestGetGroupsByLoginName()
        {
            string loginName = "zjsm";
            //string loginName = "mossmanager";

            string[] groups = ActiveDirectoryManagement.Instance.User.GetGroupsByLoginName(loginName);

            Assert.IsNotNull(groups);
        }
    }
}
