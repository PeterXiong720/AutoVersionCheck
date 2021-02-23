using System.Security.Cryptography.X509Certificates;
namespace AutomaticVersionCheckAPI.Models
{
    public class PostRequest
    {
        private string accesskey;
        public string AppName;
        public string AccessKey
        {
            get{return accesskey;}
            set{accesskey = value;}
        }
        public AppVersion Version;
    }

    public class AppVersion
    {
        public string Latest;
        public string preview;
    }

    public class PostResponce
    {
        public string AppName;
        public AppVersion version;
        public string AccessKey;
        public string result;
    }
}