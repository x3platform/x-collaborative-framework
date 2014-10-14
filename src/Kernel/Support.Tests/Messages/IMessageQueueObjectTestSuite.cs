namespace X3Platform.Tests.Messages
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;
    using System.Xml.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.Messages;
    using NMock;

    /// <summary>消息类测试类</summary>
    [TestClass]
    public class IMessageQueueObjectTestSuite
    {
        private MockFactory factory=null;

        public IMessageQueueObjectTestSuite()
		{
			// 初始化工厂类
			factory = new MockFactory();
		}

        /// <summary></summary>
        [TestMethod]
        public void TestSend()
        {
            IMessageQueueObject messageQueueObject = MessageQueueManagement.GetMessageQueueInstance("MSMQ_Messages");

            if (messageQueueObject.Enabled)
            {
                MessageObject message = new MessageObject();

                message.Result = "测试";

                // Expect.Once.On(mockMessageObject).GetProperty("Id").Will(Return.Value("00000000-0000-0000-0000-000000000001"));

                messageQueueObject.Send(message);
            }
        }

        [TestMethod]
        public void TestReceive()
        {
            IMessageQueueObject messageQueueObject = MessageQueueManagement.GetMessageQueueInstance("MSMQ_Messages");

            if (messageQueueObject.Enabled)
            {
                IMessageObject message = messageQueueObject.Receive();
            }
        }
    }
}
