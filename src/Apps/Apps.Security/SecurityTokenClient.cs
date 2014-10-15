using System;
using System.Collections;
using System.Text;
using System.Management;

namespace X3Platform.Apps.Security
{
    /// <summary>安全标记管理</summary>
    public class SecurityTokenClient
    {
        /// <summary></summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationSecretSignal"></param>
        /// <returns></returns>
        public static string CreateSecurityToken(string applicationId, string applicationSecretSignal)
        {
            // ApplicationInfo param = AppsContext.Instance.ApplicationService.FindOne(applicationId);

            //return "{\"ajaxStorage\":{\"applicationId\":\"" + param.Id + "\","
            //    + "\"applicationSecretSignal\":\"" + param.EncryptedApplicationSecretSignal + "\"}}";

            return string.Empty;
        }

        // 获取本机多个网卡号
        public static string[] GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration ");
            
            ManagementObjectCollection moc = mc.GetInstances();
            
            //****先得到网卡数目                    
            
            int i = 0;
            
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled "] == true)
                {
                    i++;
                }
                mo.Dispose();
            }

            // ***赋值给数组  
            ManagementClass mc_2 = new ManagementClass("Win32_NetworkAdapterConfiguration ");
            
            ManagementObjectCollection moc_2 = mc_2.GetInstances();
            
            string[] array = new string[i];
            
            int j = 0;

            foreach (ManagementObject mo in moc_2)
            {
                if ((bool)mo["IPEnabled "] == true)
                {
                    string delcolon = mo["MacAddress "].ToString();
                    delcolon = delcolon.Replace(": ", " ");
                    array[j] = delcolon;
                    j++;
                }
                mo.Dispose();
            }

            return array;
        }   
    }
}
