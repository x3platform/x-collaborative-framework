namespace X3Platform.Tasks.NotificationProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using com.igetui.api.openservice;
    using com.igetui.api.openservice.igetui;
    using com.igetui.api.openservice.igetui.template;
    using com.igetui.api.openservice.payload;
    using X3Platform.Configuration;
    using X3Platform.Json;
    using X3Platform.Membership;
    using X3Platform.Tasks.Configuration;
    using X3Platform.Tasks.Model;
    using X3Platform.Util;

    /// <summary>���Ʒ�����</summary>
    public class GetuiNotificationProvider : INotificationProvider
    {
        // �������ͷ����ַ
        private string Host = null;
        // ����Ӧ�õ� AppId
        private string AppId = null;
        // ����Ӧ�õ� AppKey
        private string AppKey = null;
        // ����Ӧ�õ� MasterSecret
        private string MasterSecret = null;

        public GetuiNotificationProvider()
        {
            var configuration = KernelConfigurationView.Instance.Configuration;

            this.Host = configuration.Keys["Tasks.Notification.Getui.Host"].Value;
            this.AppId = configuration.Keys["Tasks.Notification.Getui.AppId"].Value;
            this.AppKey = configuration.Keys["Tasks.Notification.Getui.AppKey"].Value;
            this.MasterSecret = configuration.Keys["Tasks.Notification.Getui.MasterSecret"].Value;
        }

        #region ����:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>����֪ͨ</summary>
        /// <param name="task">������Ϣ</param>
        /// <param name="receiverIds">������</param>
        /// <param name="options">ѡ��</param>
        public int Send(TaskWorkInfo task, string receiverIds, string options)
        {
            JsonData data = JsonMapper.ToObject(options);

            string payload = data.GetValue("payload", "{}");
            string title = data.GetValue("title", task.Tags);
            string content = data.GetValue("content", task.Title);

            // ������Ϣ
            IGtPush push = new IGtPush(this.Host, this.AppKey, this.MasterSecret);

            ListMessage message = new ListMessage();

            // �û���ǰ������ʱ���Ƿ����ߴ洢 (��ѡ)
            message.IsOffline = true;
            // ������Чʱ�䣬��λΪ���룬��ѡ  
            message.OfflineExpireTime = 1000 * 3600 * 12;
            // �ж��Ƿ�ͻ����Ƿ�wifi���������ͣ�1Ϊ��WIFI�����£�0Ϊ���������绷����
            message.PushNetWorkType = 0;

            // ƴװ͸������
            string transmissionContent = "{\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"payload\":" + payload + "}";

            message.Data = GetTransmissionTemplate(transmissionContent, this.AppId, this.AppKey);

            IList<string> clientList = MembershipManagement.Instance.AccountBindingService.FindAllBindingObjectIds(receiverIds, "Getui");

            // ���ý�����
            List<Target> targetList = new List<Target>();

            foreach (var item in clientList)
            {
                Target target1 = new Target();

                target1.appId = this.AppId;
                target1.clientId = item;
                targetList.Add(target1);
            }

            string contentId = push.getContentId(message);

            string result = push.pushMessageToList(contentId, targetList);

            return 0;
        }
        #endregion

        ///// <summary>
        /////ִ��״̬��ö��
        ///// </summary>
        //public enum PushType
        //{
        //    [DescriptionAttribute("������֤����")]
        //    FriendRquest,
        //    [DescriptionAttribute("����������")]
        //    OrderConsume,
        //    [DescriptionAttribute("֪ͨ")]
        //    Notice
        //}

        //#region ��������ȡö������������ GetEnumDescription<T>(T item)
        ///// <summary>��ȡö��������������Ϣ</summary>
        ///// <param name="item">ö��������</param>
        ///// <returns>�������</returns>
        //public string GetEnumDescription<T>(T item)
        //{
        //    string strValue = item.ToString();

        //    FieldInfo fieldinfo = item.GetType().GetField(strValue);
        //    if (fieldinfo == null)
        //    {
        //        return strValue;
        //    }

        //    Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        //    if (objs == null || objs.Length == 0)
        //    {
        //        return strValue;
        //    }
        //    else
        //    {
        //        DescriptionAttribute da = (DescriptionAttribute)objs[0];
        //        return da.Description;
        //    }
        //}
        //#endregion

        //#region ������������Ϣ��ָ���ͻ��� PushMessageToList(PushType type, string accountIds, string content, string payload)
        ///// <summary>������Ϣ��ָ���ͻ���</summary>
        ///// <param name="type">�������� ö��PushType����</param>
        ///// <param name="accountIds">�����˱�ʶ ������,�ָ�</param>
        ///// <param name="content">������Ϣ</param>
        ///// <param name="payload">͸����Ϣ JSON�ַ�����ʽ���磺{"property1":"value1","property2":"value2"}</param>
        ///// <returns>�������</returns>
        //public string PushMessageToList(string title, string content, string accountIds, string payload)
        //{
        //    IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

        //    ListMessage message = new ListMessage();
        //    // �û���ǰ������ʱ���Ƿ����ߴ洢 (��ѡ)
        //    message.IsOffline = true;
        //    // ������Чʱ�䣬��λΪ���룬��ѡ  
        //    message.OfflineExpireTime = 1000 * 3600 * 12;
        //    // �ж��Ƿ�ͻ����Ƿ�wifi���������ͣ�1Ϊ��WIFI�����£�0Ϊ���������绷����
        //    message.PushNetWorkType = 0;

        //    // ƴװ͸������
        //    string transmissionContent = "{\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"payload\":" + payload + "}";

        //    message.Data = GetTransmissionTemplate(transmissionContent, APPID, APPKEY);

        //    IList<string> pushClientList = MembershipManagement.Instance.AccountBindingService.FindAllBindingObjectIds(accountIds, "Getui");

        //    //���ý�����
        //    List<Target> targetList = new List<Target>();

        //    foreach (var item in pushClientList)
        //    {
        //        Target target1 = new Target();
        //        target1.appId = APPID;
        //        target1.clientId = item;
        //        targetList.Add(target1);
        //    }

        //    string contentId = push.getContentId(message);
        //    string result = push.pushMessageToList(contentId, targetList);

        //    return result;
        //}
        //#endregion

        #region ˽�к�����͸����Ϣģ�� GetTransmissionTemplate()
        /// <summary>͸����Ϣģ��</summary>
        /// <param name="transmissionContent">͸������</param>
        /// <returns>�������</returns>
        private TransmissionTemplate GetTransmissionTemplate(string transmissionContent, string id, string key)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = id;
            template.AppKey = key;
            // Ӧ���������ͣ�1��ǿ��Ӧ������ 2���ȴ�Ӧ������
            template.TransmissionType = "2";
            // ͸������
            template.TransmissionContent = transmissionContent;

            JsonData data = JsonMapper.ToObject(transmissionContent);

            /********************************************/
            /************** APN�߼����� **************/
            /*******************************************/
            APNPayload apnpayload = new APNPayload();

            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();

            alertMsg.Body = data["content"].ToString();
            //alertMsg.ActionLocKey = "ActionLocKey";
            //alertMsg.LocKey = "LocKey";
            //alertMsg.addLocArg("LocArg");
            //alertMsg.LaunchImage = "LaunchImage";
            ////IOS8.2֧���ֶ�
            alertMsg.Title = data["title"].ToString();
            //alertMsg.TitleLocKey = "TitleLocKey";
            //alertMsg.addTitleLocArg("TitleLocArg");

            apnpayload.AlertMsg = alertMsg;
            apnpayload.ContentAvailable = 1;
            apnpayload.Badge = 0;
            //apnpayload.Category = "";
            //apnpayload.Sound = "test1.wav";


            //foreach (JProperty jp in data["payload"])
            //{
            //    apnpayload.addCustomMsg(jp.Name, jp.Value.ToString());
            //}


            template.setAPNInfo(apnpayload);

            return template;
        }
        #endregion
    }
}
