namespace X3Platform.AttachmentStorage.Images
{
    #region Using Libraries
    using System;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>缩略图</summary>
    public class ThumbnailInfo : ICacheable
    {
        public ThumbnailInfo(string id, byte[] fileData)
        {
            this.Id = id;
            this.FileData = fileData;
        }

        #region 属性:Id
        private string m_Id;

        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:FileData
        private byte[] m_FileData;

        /// <summary>数据</summary>
        public byte[] FileData
        {
            get { return this.m_FileData; }
            set { this.m_FileData = value; }
        }
        #endregion

        //
        // 显式实现 ICacheable
        // 

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(6);

        /// <summary>过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}