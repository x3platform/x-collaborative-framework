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
    public class GroupProviderTestSuite
    {
        [TestMethod]
        public void TestGetGroupsByName()
        {
            string groupName = "集团总部";

            string[] groups = ActiveDirectoryManagement.Instance.User.GetUsersByGroupName(groupName);

            Assert.IsNotNull(groups);
        }


        [TestMethod]
        public void TestCreateGroupsByLoginName()
        {
            string groupName = "集团信息中心";

            //provider.Group.Add(groupName,);

            //Assert.IsNotNull(groups);
        }
    }
}
