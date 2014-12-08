namespace X3Platform.AttachmentStorage.Ajax
{
    #region Using Libraries
    using System;
    using System.Web;
    using System.Xml;

    using Common.Logging;

    using X3Platform.AttachmentStorage.Util;
    #endregion

    /// <summary>文件上传</summary>
    public class FileUploadWrapper : ContextWrapper
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Upload(XmlDocument doc)
        {
            this.ProcessRequest(HttpContext.Current);

            HttpContext.Current.Response.End();

            return string.Empty;
        }

        public override void ProcessRequest(HttpContext context)
        {
            //
            // 由于Flash在读取FireFox的cookie时存在bug引发以下用户验证代码发生,所以注释. 
            // [*]寻求别的验证方法替代当前方式. 
            //
            // 登录用户才可上传文件
            // if (AttachmentStorageConfigurationView.Instance.AnonymousUpload == "OFF" && KernelContext.Current.User == null)
            //    return;

            HttpRequest request = context.Request;

            HttpResponse response = context.Response;

            try
            {
                HttpPostedFile file = request.Files["fileData"];

                string attachmentId = request.Form["attachmentId"];
                string entityId = request.Form["entityId"];
                string entityClassName = request.Form["entityClassName"];
                string attachmentEntityClassName = request.Form["attachmentEntityClassName"];
                string attachmentFolder = request.Form["attachmentFolder"];

                IAttachmentParentObject parent = new AttachmentParentObject(entityId, entityClassName, attachmentEntityClassName, attachmentFolder);

                IAttachmentFileInfo attachment = UploadFileHelper.CreateAttachmentFile(parent, file);

                // Office 在线客户端上传方式
                // 支持客户端赋值附件标识
                if (!string.IsNullOrEmpty(attachmentId))
                {
                    if (AttachmentStorageContext.Instance.AttachmentFileService.IsExist(attachmentId))
                    {
                        HttpContext.Current.Response.StatusCode = 500;
                        HttpContext.Current.Response.Write("Attachment id already exists.");
                    }
                    else
                    {
                        attachment.Id = attachmentId;
                    }
                }

                attachment.Save();

                logger.Info("attachment id: " + attachment.Id);

                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.Write(attachment.Id);
            }
            catch (Exception ex)
            {
                // If any kind of error occurs return a 500 Internal Server error
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.Write("An error occured");

                logger.Error(ex.Message, ex);
            }
        }
    }
}