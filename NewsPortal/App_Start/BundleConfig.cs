using System.Web;
using System.Web.Optimization;

namespace NewsPortal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/date").Include(
                      "~/Scripts/dateTimezone.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/SearchFiltersSortScripts.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/PreviewStyle.css",
                      "~/Content/site.css"));
        }
    }
}
