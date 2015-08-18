using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Email.Client.Configuration;

namespace X3Platform.Email.Client.TestSuite.Configuration
{
    [TestClass]
    public class EmailClientConfigurationViewTestSuite 
    {
        public string fullConfigPath =  ConfigurationManager.AppSettings["fullConfigPath"];

        [TestInitialize()]
        public void Initialize()
        {
            // EmailClientConfigurationView.LoadInstance(fullConfigPath);
        }

        [TestMethod]
        public void TestLoad()
        {
            EmailClientConfiguration configuration = EmailClientConfigurationView.Instance.Configuration;

            Assert.IsNotNull(configuration);
        }
    }
}
