using X3Platform.Util;
namespace X3Platform.Web.UserAgents
{
    public sealed class UserAgent
    {
        public UserAgent(string family, string major, string minor, string patch)
        {
            Family = family;
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public string Family { get; private set; }
        public string Major { get; private set; }
        public string Minor { get; private set; }
        public string Patch { get; private set; }

        public override string ToString()
        {
            var version = StringHelper.Join(".", Major, Minor, Patch);
            return Family + (!string.IsNullOrEmpty(version) ? " " + version : null);
        }
    }
}
