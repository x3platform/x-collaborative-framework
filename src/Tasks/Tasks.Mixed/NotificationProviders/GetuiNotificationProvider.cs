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

    /// <summary>个推发送器</summary>
    public class GetuiNotificationProvider : INotificationProvider
    {
        // 个推推送服务地址
        private string Host = null;
        // 个推应用的 AppId
        private string AppId = null;
        // 个推应用的 AppKey
        private string AppKey = null;
        // 个推应用的 MasterSecret
        private string MasterSecret = null;

        public GetuiNotificationProvider()
        {
            var configuration = KernelConfigurationView.Instance.Configuration;

            this.Host = configuration.Keys["Tasks.Notification.Getui.Host"].Value;
            this.AppId = configuration.Keys["Tasks.Notification.Getui.AppId"].Value;
            this.AppKey = configuration.Keys["Tasks.Notification.Getui.AppKey"].Value;
            this.MasterSecret = configuration.Keys["Tasks.Notification.Getui.MasterSecret"].Value;
        }

        #region 函数:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>发送通知</summary>
        /// <param name="task">任务信息</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="options">选项</param>
        public int Send(TaskWorkInfo task, string receiverIds, string options)
        {
            JsonData data = JsonMapper.ToObject(options);

            string payload = data.GetValue("payload", "{}");
            string title = data.GetValue("title", task.Tags);
            string content = data.GetValue("content", task.Title);

            // 推送消息
            IGtPush push = new IGtPush(this.Host, this.AppKey, this.MasterSecret);

            ListMessage message = new ListMessage();

            // 用户当前不在线时，是否离线存储 (可选)
            message.IsOffline = true;
            // 离线有效时间，单位为毫秒，可选  
            message.OfflineExpireTime = 1000 * 3600 * 12;
            // 判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为不限制网络环境。
            message.PushNetWorkType = 0;

            // 拼装透传内容
            string transmissionContent = "{\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"payload\":" + payload + "}";

            message.Data = GetTransmissionTemplate(transmissionContent, this.AppId, this.AppKey);

            IList<string> clientList = MembershipManagement.Instance.AccountBindingService.FindAllBindingObjectIds(receiverIds, "Getui");

            // 设置接收者
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
        /////执行状态的枚举
        ///// </summary>
        //public enum PushType
        //{
        //    [DescriptionAttribute("好友验证申请")]
        //    FriendRquest,
        //    [DescriptionAttribute("订单已消费")]
        //    OrderConsume,
        //    [DescriptionAttribute("通知")]
        //    Notice
        //}

        //#region 函数：获取枚举类子项属性 GetEnumDescription<T>(T item)
        ///// <summary>获取枚举类子项属性信息</summary>
        ///// <param name="item">枚举类子项</param>
        ///// <returns>操作结果</returns>
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

        //#region 函数：推送消息到指定客户端 PushMessageToList(PushType type, string accountIds, string content, string payload)
        ///// <summary>推送消息到指定客户端</summary>
        ///// <param name="type">推送类型 枚举PushType类型</param>
        ///// <param name="accountIds">接收人标识 多人用,分隔</param>
        ///// <param name="content">推送信息</param>
        ///// <param name="payload">透传信息 JSON字符串格式，如：{"property1":"value1","property2":"value2"}</param>
        ///// <returns>操作结果</returns>
        //public string PushMessageToList(string title, string content, string accountIds, string payload)
        //{
        //    IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

        //    ListMessage message = new ListMessage();
        //    // 用户当前不在线时，是否离线存储 (可选)
        //    message.IsOffline = true;
        //    // 离线有效时间，单位为毫秒，可选  
        //    message.OfflineExpireTime = 1000 * 3600 * 12;
        //    // 判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为不限制网络环境。
        //    message.PushNetWorkType = 0;

        //    // 拼装透传内容
        //    string transmissionContent = "{\"title\":\"" + title + "\",\"content\":\"" + content + "\",\"payload\":" + payload + "}";

        //    message.Data = GetTransmissionTemplate(transmissionContent, APPID, APPKEY);

        //    IList<string> pushClientList = MembershipManagement.Instance.AccountBindingService.FindAllBindingObjectIds(accountIds, "Getui");

        //    //设置接收者
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

        #region 私有函数：透传消息模板 GetTransmissionTemplate()
        /// <summary>透传消息模板</summary>
        /// <param name="transmissionContent">透传内容</param>
        /// <returns>操作结果</returns>
        private TransmissionTemplate GetTransmissionTemplate(string transmissionContent, string id, string key)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = id;
            template.AppKey = key;
            // 应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionType = "2";
            // 透传内容
            template.TransmissionContent = transmissionContent;

            JsonData data = JsonMapper.ToObject(transmissionContent);

            /********************************************/
            /************** APN高级推送 **************/
            /*******************************************/
            APNPayload apnpayload = new APNPayload();

            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();

            alertMsg.Body = data["content"].ToString();
            //alertMsg.ActionLocKey = "ActionLocKey";
            //alertMsg.LocKey = "LocKey";
            //alertMsg.addLocArg("LocArg");
            //alertMsg.LaunchImage = "LaunchImage";
            ////IOS8.2支持字段
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
