using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Optimization;
using NewProject.Site.Code;

namespace LComplete.Framework.Site.Web.UI
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new CdnScriptBundle("~/scripts/jquery").Include("~/Scripts/jquery-{version}.js"));
        }
    }

    public class CdnStyleBundle : StyleBundle
    {
        public string MyCdnPath { get; set; }

        public CdnStyleBundle(string virtualPath)
            : base(virtualPath)
        {
            MyCdnPath = "//" + AppConfig.CdnDomain + virtualPath.TrimStart('~');
        }

        public CdnStyleBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath)
        {
            MyCdnPath = cdnPath;
        }

        public override BundleResponse ApplyTransforms(BundleContext context, string bundleContent, IEnumerable<BundleFile> bundleFiles)
        {
            BundleResponse response = base.ApplyTransforms(context, bundleContent, bundleFiles);
            base.CdnPath = string.Format("{0}?v={1}", MyCdnPath, GetContentHash(response.Content));

            return response;
        }

        private string GetContentHash(string content)
        {
            using (SHA256 sha = new SHA256Managed())
            {
                return HttpServerUtility.UrlTokenEncode(sha.ComputeHash(Encoding.Unicode.GetBytes(content)));
            }
        }
    }

    public class CdnScriptBundle : ScriptBundle
    {
        public string MyCdnPath { get; set; }

        public CdnScriptBundle(string virtualPath)
            : base(virtualPath)
        {
            MyCdnPath = "//" + AppConfig.CdnDomain + virtualPath.TrimStart('~');
        }

        public CdnScriptBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath)
        {
            MyCdnPath = cdnPath;
        }

        public override BundleResponse ApplyTransforms(BundleContext context, string bundleContent, IEnumerable<BundleFile> bundleFiles)
        {
            BundleResponse response = base.ApplyTransforms(context, bundleContent, bundleFiles);
            base.CdnPath = string.Format("{0}?v={1}", MyCdnPath, GetContentHash(response.Content));

            return response;
        }

        private string GetContentHash(string content)
        {
            using (SHA256 sha = new SHA256Managed())
            {
                return HttpServerUtility.UrlTokenEncode(sha.ComputeHash(Encoding.Unicode.GetBytes(content)));
            }
        }
    }

}