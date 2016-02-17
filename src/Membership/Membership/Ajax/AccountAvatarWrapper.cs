namespace X3Platform.Membership.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Text;
    using System.Web;
    using System.IO;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    
    using X3Platform.Ajax;
    using X3Platform.Configuration;
    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Location.IPQuery;
    using X3Platform.Messages;
    using X3Platform.Sessions;
    using X3Platform.Util;
    
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    public class AccountAvatarWrapper
    {
        /// <summary>上传头像</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string Upload(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string data = XmlHelper.Fetch("data", doc);

            Bitmap bitmap = null;

            int height;
            int width;

            IAccountInfo account = KernelContext.Current.User;

            if (account == null)
            {
                return MessageObject.Stringify("1", "必须登陆后才能上传头像。");
            }

            string directoryName = this.GetDirectoryName(account.Id);

            string path = MembershipConfigurationView.Instance.AvatarPhysicalFolder + directoryName + "\\";

            string filePath = null;

            if (!Directory.Exists(path))
            {
                DirectoryHelper.Create(path);
            }

            byte[] buffer = Convert.FromBase64String(data);

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                bitmap = new Bitmap(stream);
            }

            string[] sizes = MembershipConfigurationView.Instance.AvatarSizes.Split(',');

            height = width = Convert.ToInt32(sizes[0]);

            filePath = string.Format(@"{0}\{1}_" + height + "x" + width + ".png", path, account.Id);

            bitmap = KiResizeImage(bitmap, width, height);

            bitmap.Save(filePath, ImageFormat.Png);

            // 设置头像记录
            filePath = account.CertifiedAvatar = "{avatar}" + directoryName + "/" + account.Id + "_" + height + "x" + width + ".png";

            MembershipManagement.Instance.AccountService.SetCertifiedAvatar(account.Id, filePath);

            IAccountStorageStrategy strategy = KernelContext.Current.AuthenticationManagement.GetAccountStorageStrategy();

            string accountIdentity = KernelContext.Current.AuthenticationManagement.GetIdentityValue();

            SessionContext.Instance.Write(strategy, accountIdentity, account);

            // 保存其他格式图片
            for (int i = 1; i < sizes.Length; i++)
            {
                height = width = Convert.ToInt32(sizes[i]);

                filePath = string.Format(@"{0}\{1}_" + height + "x" + width + ".png", path, account.Id);

                bitmap = KiResizeImage(bitmap, height, height);

                bitmap.Save(filePath, ImageFormat.Png);
            }

            // 记录帐号操作日志
            MembershipManagement.Instance.AccountLogService.Log(account.Id, "membership.account.avatar.upload", "【" + account.Name + "】更新了自己的头像，【IP:" + IPQueryContext.GetClientIP() + "】。", account.Id);

            return "{\"data\":{\"virtualPath\":\"" + account.CertifiedAvatar.Replace("{avatar}", MembershipConfigurationView.Instance.AvatarVirtualFolder).Replace("//", "/") + "\"},\"message\":{\"returnCode\":0,\"value\":\"上传成功。\"}}";
        }

        public Bitmap BuildBitmap(int width, int height, string strBmp)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            byte[] buffer = Convert.FromBase64String(strBmp);

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return new Bitmap(stream);
            }

            // string[] StmpBmp = strBmp.Split(',');

            int position = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // bitmap.SetPixel(x, y, Color.FromArgb(int.Parse(StmpBmp[position], System.Globalization.NumberStyles.HexNumber)));
                    bitmap.SetPixel(x, y, Color.FromArgb(buffer[position]));
                    position++;
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="Mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary></summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetDirectoryName(string index)
        {
            // 制作个简化的模拟函数生成一个整数共对象取模操作

            int hash = 0;

            byte[] buffer = Encoding.UTF8.GetBytes(index);

            // 将字节相加获得一个整数
            for (int i = 0; i < buffer.Length; i++)
            {
                // (hash >> 5) => hasH * (2^5) => hash * 32
                // 位运算的速度比较快 
                hash = (hash >> 5) + buffer[i];
            }

            // 将负数转化正数
            hash = (hash & 0x7FFFFFFF) % 999;

            return hash.ToString("000");
        }
    }
}
