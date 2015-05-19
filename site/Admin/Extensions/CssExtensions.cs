using System.Web.Mvc;

namespace Admin.Extensions
{
    public static class CssExtensions
    {
        public static string ValidateCssFor(this HtmlHelper htmlHelper,string filed,bool getSuccessCss=false)
        {
            if (htmlHelper.ViewData.ModelState.IsValidField(filed))
            {
                return getSuccessCss ? "success" : string.Empty;
            }
            return "error";
        }
    }
}