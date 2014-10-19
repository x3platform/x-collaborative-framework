namespace X3Platform.AttachmentStorage.Images
{
    #region Using Libraries
    using System;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>����ͼ</summary>
    public class ThumbnailInfo : ICacheable
    {
        public ThumbnailInfo(string id, byte[] fileData)
        {
            this.Id = id;
            this.FileData = fileData;
        }

        #region ����:Id
        private string m_Id;

        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region ����:FileData
        private byte[] m_FileData;

        /// <summary>����</summary>
        public byte[] FileData
        {
            get { return this.m_FileData; }
            set { this.m_FileData = value; }
        }
        #endregion

        //
        // ��ʽʵ�� ICacheable
        // 

        #region ����:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(6);

        /// <summary>����ʱ��</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}