namespace X3Platform.AttachmentStorage.Ajax
{
    #region Using Libraries
    using System;
    using System.Drawing;
    using System.Web;
    using System.Xml;

    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Images;
    using X3Platform.AttachmentStorage.Util;
    using X3Platform.AttachmentStorage.Configuration;
    #endregion

    /// <summary>图片上传</summary>
    public class ImageUploadWrapper : ContextWrapper
    {
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
            if (KernelContext.Current.User == null)
                return;

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

                int resize = request.Form["targetWidth"] == null ? 0 : Convert.ToInt32(request.Form["resize"]);
                int targetWidth = request.Form["targetWidth"] == null ? 0 : Convert.ToInt32(request.Form["targetWidth"]);
                int targetHeight = request.Form["targetHeight"] == null ? 0 : Convert.ToInt32(request.Form["targetHeight"]);

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

                // 调整图片大小
                if (resize == 1)
                {
                    Image image = Image.FromStream(ByteHelper.ToStream(attachment.FileData));

                    attachment.FileData = ThumbnailManagement.Resize(image, attachment.FileType, targetWidth, targetHeight);
                }

                attachment.Save();

                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.Write(attachment.Id);
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