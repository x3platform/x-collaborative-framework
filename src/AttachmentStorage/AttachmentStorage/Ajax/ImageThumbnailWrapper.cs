// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.AttachmentStorage.Ajax
{
    using System;
    using System.Web;
    using System.IO;
    using System.Xml;

    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Images;

    /// <summary>图片缩略图</summary>
    public sealed class ImageThumbnailWrapper : ContextWrapper
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

            string fileName = request.QueryString["fileName"] == null ? string.Empty : request.QueryString["fileName"];

            string id = request.QueryString["id"] == null ? string.Empty : request.QueryString["id"];

            int targetWidth = request.QueryString["targetWidth"] == null ? 100 : Convert.ToInt32(request.QueryString["targetWidth"]);
            int targetHeight = request.QueryString["targetHeight"] == null ? 100 : Convert.ToInt32(request.QueryString["targetHeight"]);

            string attachmentEntityClassName = request.QueryString["attachmentEntityClassName"] == null ? "X3Platform.AttachmentStorage.AttachmentFileInfo" : request.QueryString["attachmentEntityClassName"];

            if (string.IsNullOrEmpty(fileName))
            {
                // 以参数形式生成图片
                if (!string.IsNullOrEmpty(id))
                {
                    // 读取附件原始信息

                    IAttachmentFileInfo attachment = (IAttachmentFileInfo)SpringContext.Instance.Application.GetObject(attachmentEntityClassName); ;

                    attachment.Restore(id);

                    if (attachment.FileData == null)
                    {
                        response.StatusCode = 404;
                        response.Write("Not Found");
                        return;
                    }

                    string defaultThumbnail = string.Format("{0}_{1}x{2}", attachment.FileType.Replace(".", ""), targetWidth, targetHeight);

                    if (AttachmentStorageConfigurationView.Instance.DefaultThumbnails.IndexOf(defaultThumbnail) > -1)
                    {
                        response.Redirect(AttachmentStorageConfigurationView.Instance.VirtualUploadFolder + "thumbnails/" + defaultThumbnail + ".png");
                        response.End();
                    }

                    fileName = string.Format("{0}_{1}x{2}{3}", id, targetWidth, targetHeight, attachment.FileType);

                    Stream stream = ByteHelper.ToStream(attachment.FileData);

                    // 创建缩略图
                    ThumbnailManagement.CreateThumbnail(id, stream, attachment.FileType, targetWidth, targetHeight);
                }
            }
            else
            {
                // 直接以文件名方式访问
                if (!ThumbnailManagement.IsExist(fileName))
                {
                    response.StatusCode = 404;
                    response.Write("Not Found");
                    return;
                }
            }

            response.Redirect(AttachmentStorageConfigurationView.Instance.VirtualUploadFolder + "thumbnails/" + fileName);
        }
    }
}