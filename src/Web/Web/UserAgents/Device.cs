namespace X3Platform.Web.UserAgents
{
    /// <summary>设备信息</summary>
    public sealed class Device
    {
        public Device(string family, bool isSpider)
        {
            Family = family;
            IsSpider = isSpider;
        }

        public string Family { get; private set; }
        public bool IsSpider { get; private set; }

        public override string ToString() { return Family; }
    }
}
