using System.Configuration;

namespace NewProject.Site.Code
{
    public class AppConfig
    {
        public static string CdnDomain
        {
            get { return ConfigurationManager.AppSettings["CdnDomain"] ?? "static.website.com"; }
        }
    }
}