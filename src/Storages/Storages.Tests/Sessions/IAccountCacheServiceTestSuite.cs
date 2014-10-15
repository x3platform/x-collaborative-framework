namespace X3Platform.Storages.TestSuite.Sessions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using X3Platform.AccountCache;
    using System.Text;
    using System.Security.Cryptography;

    [TestClass]
    public class IAccountCacheServiceTestSuite
    {
        [TestMethod]
        public void TestInsert()
        {
            /*
            AccountCacheInfo param = new AccountCacheInfo();

            param.AccountIdentity = "test_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            param.AccountCacheValue = "test";

            param.AccountObject = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<accountObject><id>fbe67a6e-4c6c-439e-b54b-f62af23d30ba</id><loginName>jiqiliang</loginName><name>������</name><ip>127.0.0.1</ip></accountObject>";

            param.AccountObjectType = "X3Platform.Membership.Model.AccountInfo,X3Platform.Membership";
            
            param.IP = "255.255.255.255";

            param.BeginDate = param.EndDate = DateTime.Now;

            param.UpdateDate = DateTime.Now;

            AccountCacheContext.Instance.AccountCacheService.Insert(param);

            AccountCacheInfo result = AccountCacheContext.Instance.AccountCacheService.FindByAccountIdentity(param.AccountIdentity);

            Assert.IsTrue(param.AccountIdentity == result.AccountIdentity);
        */
        }

        [TestMethod]
        public void TestDump()
        {
            /* 
            IList<AccountCacheInfo> list = AccountCacheContext.Instance.AccountCacheService.Dump();

            Assert.IsTrue(list.Count > 0);
        */
        }

        [TestMethod]
        public void test1()
        {
            string text = string.Empty;
            Assert.AreEqual(GetHashCode(text), 0);
            text = "abc";
            text.GetHashCode();
            Assert.AreEqual(GetHashCode(text), 277766);
            text = "abcd";
            text.GetHashCode();
            Assert.AreEqual(GetHashCode(text), 14721698);
            int test1 = "12345".GetHashCode();
            int test3 = "123456".GetHashCode();
            text = "0f2fd5c2-9914-4b25-8cba-399640d84857";
            text.GetHashCode();
            Assert.AreEqual(GetHashCode(text), 1562818641);
            text = "63c49c16-cdb4-4f70-b626-91c269101bef";
            text.GetHashCode();
            Assert.AreEqual(GetHashCode(text), 1565444782);
            string[] indexs = new string[] { "0f2fd5c2-9914-4b25-8cba-399640d84857", "63c49c16-cdb4-4f70-b626-91c269101bef", "6c069a8e-9527-44c7-8999-f1b0444794a6", "e28276da-694f-45f9-bc6e-1cf7bfb7c33e" };
            Assert.AreEqual(GetHashCode(indexs), 1565444782);
            
            string test = HashStringBySHA1("12345");

            string sSourceData;
            byte[] tmpSource;
            byte[] tmpHash;
            sSourceData = "MySourceData";
            //Create a byte array from source data
            tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

            //Compute hash based on source data
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            Console.WriteLine(ByteArrayToString(tmpHash));

            sSourceData = "NotMySourceData";
            tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

            byte[] tmpNewHash;

            tmpNewHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            bool bEqual = false;
            if (tmpNewHash.Length == tmpHash.Length)
            {
                int i = 0;
                while ((i < tmpNewHash.Length) && (tmpNewHash[i] == tmpHash[i]))
                {
                    i += 1;
                }
                if (i == tmpNewHash.Length)
                {
                    bEqual = true;
                }
            }

            if (bEqual)
                Console.WriteLine("The two hash values are the same");
            else
                Console.WriteLine("The two hash values are not the same");
            Console.ReadLine();
        }

        static string ByteArrayToString(byte[] arrInput)
        {
            int i;

            StringBuilder sOutput = new StringBuilder(arrInput.Length);

            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }

            return sOutput.ToString();
        }

        /// <summary>  
        /// �����ַ����� SHA1 ��ϣֵ��  
        ///   
        /// ��ϣ���������ⳤ�ȵĶ������ַ���ӳ��Ϊ�̶����ȵ�С�Ͷ������ַ�����  
        /// ���ܹ�ϣ����������һ�����ԣ��ڼ����ϲ�������ҵ�ɢ��Ϊ��ͬ��ֵ��  
        /// ������ͬ�����룻Ҳ����˵���������ݵĹ�ϣֵ���ڶ�Ӧ������Ҳƥ��ʱ  
        /// �Ż�ƥ�䡣���ݵ��������Ļ��ڹ�ϣֵ�в�������Ԥ֪�Ĵ������ġ�  
        ///   
        /// SHA1CryptoServiceProvider ��Ĺ�ϣ��СΪ 160 λ��  
        /// </summary>  
        /// <param name="s">��ϣǰ���ַ���</param>  
        /// <returns>��ϣ����ַ���</returns>  
        private string HashStringBySHA1(string s)
        {
            // ���ַ�������Ϊһ���ֽ�����  
            byte[] bufferValue = Encoding.UTF8.GetBytes(s);
            // ������ܹ�ϣ�㷨�����࣬��System.Security.Cryptography �����ռ� ��  
            HashAlgorithm ha = new SHA1CryptoServiceProvider();
            // ����ָ���ֽ�����Ĺ�ϣֵ  
            byte[] bufferHash = ha.ComputeHash(bufferValue);
            // �ͷ��� HashAlgorithm ��ʹ�õ�������Դ  
            ha.Clear();

            // �� 8 λ�޷������������ֵת��Ϊ���ĵ�Ч String ��ʾ��ʽ��ʹ�� base 64 ���ֱ��룩��  
            return Convert.ToBase64String(bufferHash);
        }


        private int GetHashCode(string[] indexs)
        {
            if (indexs.Length == 0)
            {
                return 1;
            }
            else
            {
                int hash = GetHashCode(indexs[0]);

                for (int i = 1; i < indexs.Length; i++)
                {
                    hash += GetHashCode(indexs[i]);
                }

                return hash & 0x7FFFFFFF;
            }
        }

        private int GetHashCode(string index)
        {
            int hash = 0;

            byte[] buffer = Encoding.UTF8.GetBytes(index);
            
            for (int i = 0; i < buffer.Length; i++)
            {
                hash = 53 * hash + buffer[i];
            }

            // ������ת������
            return hash & 0x7FFFFFFF;
        }
    }
}
