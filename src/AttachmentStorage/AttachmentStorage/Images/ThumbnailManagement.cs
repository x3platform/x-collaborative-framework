namespace X3Platform.AttachmentStorage.Images
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Drawing;
    using X3Platform.AttachmentStorage.Configuration;
    using System.Drawing.Imaging;
    #endregion

    /// <summary>缩略图管理</summary>
    public sealed class ThumbnailManagement
    {
        public static bool IsExist(string fileName)
        {
            return File.Exists(AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder + "thumbnails/" + fileName);
        }

        public static void CreateThumbnail(Stream stream, string fileType, int targetWidth, int targetHeight)
        {
            Image image = System.Drawing.Image.FromStream(stream);

            CreateThumbnail(image, fileType, targetWidth, targetHeight);
        }

        public static void CreateThumbnail(Image image, string fileType, int targetWidth, int targetHeight)
        {
            string thumbnailId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            CreateThumbnail(thumbnailId, image, fileType, targetWidth, targetHeight);
        }

        public static void CreateThumbnail(string thumbnailId, Stream stream, string fileType, int targetWidth, int targetHeight)
        {
            Image image = System.Drawing.Image.FromStream(stream);

            CreateThumbnail(thumbnailId, image, fileType, targetWidth, targetHeight);
        }

        public static void CreateThumbnail(string thumbnailId, Image image, string fileType, int targetWidth, int targetHeight)
        {
            // 计算缩略图的宽度和高度
            int width = image.Width;
            int height = image.Height;

            int thumbnailWidth, thumbnailHeight;

            float targetRatio = (float)targetWidth / (float)targetHeight;
            float imageRatio = (float)width / (float)height;

            if (targetRatio > imageRatio)
            {
                thumbnailHeight = targetHeight;
                thumbnailWidth = (int)Math.Floor(imageRatio * (float)targetHeight);
            }
            else
            {
                thumbnailHeight = (int)Math.Floor((float)targetWidth / imageRatio);
                thumbnailWidth = targetWidth;
            }

            thumbnailWidth = thumbnailWidth > targetWidth ? targetWidth : thumbnailWidth;
            thumbnailHeight = thumbnailHeight > targetHeight ? targetHeight : thumbnailHeight;

            // 创建缩略图
            using (Bitmap finalImage = new Bitmap(targetWidth, targetHeight))
            {
                using (Graphics graphic = Graphics.FromImage(finalImage))
                {
                    graphic.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, targetWidth, targetHeight));

                    int pasteX = (targetWidth - thumbnailWidth) / 2;
                    int pasteY = (targetHeight - thumbnailHeight) / 2;

                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */

                    //graphic.DrawImage(thumbnailImage, pasteX, pasteY, thumbnailWidth, thumbnailHeight);
                    graphic.DrawImage(image, pasteX, pasteY, thumbnailWidth, thumbnailHeight);

                    string path = AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder + "thumbnails/" + string.Format("{0}_{1}x{2}{3}", thumbnailId, targetWidth, targetHeight, fileType);

                    finalImage.Save(path, ParseImageFormatValue(fileType));
                }
            }
        }

        public static byte[] Resize(Image image, string fileType, int targetWidth, int targetHeight)
        {
            byte[] buffer = null;

            // 计算缩略图的宽度和高度
            int width = image.Width;
            int height = image.Height;

            int resizeWidth, resizeHeight;

            float targetRatio = (float)targetWidth / (float)targetHeight;
            float imageRatio = (float)width / (float)height;

            if (targetRatio > imageRatio)
            {
                resizeHeight = targetHeight;
                resizeWidth = (int)Math.Floor(imageRatio * (float)targetHeight);
            }
            else
            {
                resizeHeight = (int)Math.Floor((float)targetWidth / imageRatio);
                resizeWidth = targetWidth;
            }

            resizeWidth = resizeWidth > targetWidth ? targetWidth : resizeWidth;
            resizeHeight = resizeHeight > targetHeight ? targetHeight : resizeHeight;

            // 生成图片
            using (Bitmap finalImage = new Bitmap(resizeWidth, resizeHeight))
            {
                using (Graphics graphic = Graphics.FromImage(finalImage))
                {
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */

                    graphic.DrawImage(image, 0, 0, resizeWidth, resizeHeight);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        finalImage.Save(memoryStream, ParseImageFormatValue(fileType));

                        buffer = memoryStream.GetBuffer();
                    }
                }
            }

            return buffer;
        }

        /// <summary>根据文件名后缀获取ImageFormat类的值</summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static ImageFormat ParseImageFormatValue(string fileType)
        {
            switch (fileType.ToLower())
            {
                case ".ico":
                    return ImageFormat.Icon;
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                case ".jpg":
                case ".jpeg":
                default:
                    return ImageFormat.Jpeg;
            }
        }
    }
}