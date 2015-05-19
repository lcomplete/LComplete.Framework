using System.Web.Optimization;

namespace Admin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include("~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/bootstrap-datepicker.zh-CN.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bt").Include("~/Scripts/plugin/table/bootstrap-table.js"
                //"~/Scripts/plugin/table/bootstrap-table-export.js",
                //"~/Scripts/plugin/table/tableExport.js",
                //"~/Scripts/plugin/table/jquery.base64.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/validate").Include("~/Scripts/jquery.validate.js",
                                                                       "~/Scripts/jquery.validate.unobtrusive.js"));


            bundles.Add(new ScriptBundle("~/Scripts/multiSelect").Include("~/Scripts/jquery.multi-select.js"));
            bundles.Add(new StyleBundle("~/content/plugin/multiSelect/css").Include("~/content/plugin/multiSelect/multi-select.css"));

            bundles.Add(
                new ScriptBundle("~/Scripts/flot").Include("~/Scripts/plugin/flot/excanvas.js", "~/Scripts/plugin/flot/jquery.flot.js").IncludeDirectory(
                    "~/Scripts/plugin/flot", "jquery.*"));

            bundles.Add(new StyleBundle("~/Content/theme/css/all").Include("~/Content/theme/css/bootstrap.css",
                                                                 "~/Content/theme/css/theme.css",
                                                                 "~/Content/theme/css/font-awesome.css",
                                                                 "~/Scripts/plugin/table/bootstrap-table.css",
                                                                 "~/Content/theme/css/datepicker.css"));
        }
    }
}