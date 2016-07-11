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
    using System.Text.RegularExpressions;
    using System.IO;
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
            HttpRequest request = context.Request;

            HttpResponse response = context.Response;

            try
            {
                HttpPostedFile file = request.Files["fileData"];

                // 输出类型 id uri base64
                string outputType = request.Form["outputType"];

                string attachmentId = request.Form["attachmentId"];
                string entityId = request.Form["entityId"];
                string entityClassName = request.Form["entityClassName"];
                string attachmentEntityClassName = request.Form["attachmentEntityClassName"];
                string attachmentFolder = request.Form["attachmentFolder"];

                // 调整图片大小
                int resize = request.Form["resize"] == null ? 0 : Convert.ToInt32(request.Form["resize"]);
                int targetWidth = request.Form["targetWidth"] == null ? 0 : Convert.ToInt32(request.Form["targetWidth"]);
                int targetHeight = request.Form["targetHeight"] == null ? 0 : Convert.ToInt32(request.Form["targetHeight"]);

                // 设置图片缩略图
                int thumbnail = request.Form["thumbnail"] == null ? 0 : Convert.ToInt32(request.Form["thumbnail"]);
                int thumbnailWidth = request.QueryString["targetWidth"] == null ? 100 : Convert.ToInt32(request.QueryString["thumbnailWidth"]);
                int thumbnailHeight = request.QueryString["thumbnailHeight"] == null ? 100 : Convert.ToInt32(request.QueryString["thumbnailHeight"]);

                // 匿名用户上传文件的文件, 存放在 anonymous 目录
                if (KernelContext.Current.User == null) { attachmentFolder = "anonymous"; }

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
                    // 调整图片大小
                    if (resize == 1)
                    {
                        Image image = Image.FromStream(ByteHelper.ToStream(attachment.FileData));

                        attachment.FileData = ThumbnailManagement.Resize(image, attachment.FileType, targetWidth, targetHeight);
                    }

                    if (outputType == "base64")
                    {
                        // 输出 Base64
                        HttpContext.Current.Response.StatusCode = 200;

                        HttpContext.Current.Response.Write(ByteHelper.ToBase64(attachment.FileData));
                    }
                    else
                    {
                        attachment.Save();

                        // 调整图片大小
                        if (thumbnail == 1)
                        {
                            Image image = Image.FromStream(ByteHelper.ToStream(attachment.FileData));

                            attachment.FileData = ThumbnailManagement.Resize(image, attachment.FileType, thumbnailWidth, thumbnailHeight);

                            var fileName = string.Format("{0}_{1}x{2}{3}", attachment.Id, thumbnailWidth, thumbnailHeight, attachment.FileType);

                            Stream stream = ByteHelper.ToStream(attachment.FileData);

                            // 创建缩略图
                            ThumbnailManagement.CreateThumbnail(attachment.Id, stream, attachment.FileType, thumbnailWidth, thumbnailHeight);
                        }

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