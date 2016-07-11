namespace X3Platform.AttachmentStorage.Ajax
{
    #region Using Libraries
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using Common.Logging;
    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Util;
    #endregion

    /// <summary>文件上传</summary>
    public class FileUploadWrapper : ContextWrapper
    {
        public string Upload(XmlDocument doc)
        {
            this.ProcessRequest(HttpContext.Current);

            HttpContext.Current.Response.End();

            return string.Empty;
        }

        public override void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;

            HttpResponse response = context.Response;

            try
            {
                foreach (string key in request.ServerVariables.Keys)
                {
                    KernelContext.Log.Info(key + ":" + request.ServerVariables[key]);
                }

                foreach (string key in request.Form.Keys)
                {
                    KernelContext.Log.Info(key + ":" + request.Form[key]);
                }

                foreach (string key in request.Files.Keys)
                {
                    KernelContext.Log.Info(key + ":" + request.Files[key].FileName);
                }

                KernelContext.Log.Info("file count:" + request.Files.Count);

                HttpPostedFile file = request.Files["fileData"];

                // 输出类型 id uri base64
                string outputType = request.Form["outputType"];

                string attachmentId = request.Form["attachmentId"];
                string entityId = request.Form["entityId"];
                string entityClassName = request.Form["entityClassName"];
                string attachmentEntityClassName = request.Form["attachmentEntityClassName"];
                string attachmentFolder = request.Form["attachmentFolder"];

                // 匿名用户上传文件的文件, 存放在 anonymous 目录
                if (KernelContext.Current.User == null) { attachmentFolder = "anonymous"; }

                // IAttachmentParentObject parent = new AttachmentParentObject(entityId, entityClassName, attachmentEntityClassName, attachmentFolder);

                IAttachmentFileInfo attachment = UploadFileHelper.CreateAttachmentFile(
                    entityId,
                    entityClassName,
                    attachmentEntityClassName,
                    attachmentFolder,
                    file);

                // Office 在线客户端上传方式
                // 支持客户端赋值附件标识
                if (!string.IsNullOrEmpty(attachmentId))
                {
                    if (AttachmentStorageContext.Instance.AttachmentFileService.IsExist(attachmentId))
                    {
                        HttpContext.Current.Response.StatusCode = 500;
                        HttpContext.Current.Response.StatusDescription = "Attachment id already exists.";
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        attachment.Id = attachmentId;
                    }
                }

                // 检测文件最小限制
                if (attachment.FileSize < AttachmentStorageConfigurationView.Instance.AllowMinFileSize)
                {
                    HttpContext.Current.Response.StatusCode = 500;
                    HttpContext.Current.Response.StatusDescription = "Attachment file size is too small.";
                    HttpContext.Current.Response.End();
                }

                // 检测文件最大限制
                if (attachment.FileSize > AttachmentStorageConfigurationView.Instance.AllowMaxFileSize)
                {
                    HttpContext.Current.Response.StatusCode = 500;
                    HttpContext.Current.Response.StatusDescription = "Attachment file size is too big.";
                    HttpContext.Current.Response.End();
                }

                // 检测文件名后缀限制
                if (!Regex.IsMatch(attachment.FileType, AttachmentStorageConfigurationView.Instance.AllowFileTypes))
                {
                    HttpContext.Current.Response.StatusCode = 500;
                    HttpContext.Current.Response.StatusDescription = "Attachment file type is invalid.";
                    HttpContext.Current.Response.End();
                }

                if (HttpContext.Current.Response.StatusCode != 500)
                {
                    attachment.Save();

                    KernelContext.Log.Info("attachment id: " + attachment.Id);

                    HttpContext.Current.Response.StatusCode = 200;

                    if (outputType == "uri")
                    {
                        // 输出 uri
                        HttpContext.Current.Response.Write(attachment.VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.VirtualUploadFolder));
                    }
                    else
                    {
                        // 输出 id
                        HttpContext.Current.Response.Write(attachment.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                // If any kind of error occurs return a 500 Internal Server error
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "An error occured";
                HttpContext.Current.Response.End();

                KernelContext.Log.Error(ex.Message, ex);
            }
        }
    }
}