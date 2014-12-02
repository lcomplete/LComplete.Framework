using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using LComplete.Framework.Data;
using LComplete.Framework.Web.Common;

namespace LComplete.Framework.Web.Mvc
{
    /// <summary>
    /// 提供一组用于分页的扩展方法。
    /// </summary>
    public static class PagerExtensions
    {
        internal const string PAGE_INDEX_NAME = "page";

        public static MvcHtmlString Pager(this HtmlHelper<PagingModel> htmlHelper, PagingModel pagedEntity)
        {
            return GeneratePagerHtml(htmlHelper, htmlHelper.ViewContext.RequestContext, pagedEntity);
        }

        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, PagingModel pagedEntity)
        {
            return GeneratePagerHtml(htmlHelper, htmlHelper.ViewContext.RequestContext, pagedEntity);
        }

        private static MvcHtmlString GeneratePagerHtml(HtmlHelper htmlHelper, RequestContext context, PagingModel pagedEntity)
        {
            if (pagedEntity == null)
                throw new ArgumentNullException("pagedEntity");

            StringBuilder html = new StringBuilder();

            int pageNumber = pagedEntity.PageIndex;
            int totalItemCount = pagedEntity.RecordCount;
            int totalpage = (int)Math.Ceiling(totalItemCount / (double)pagedEntity.PageSize);
            int currentPageIndex = pagedEntity.PageIndex;
            bool hasPreviousPage = currentPageIndex > 1;
            bool hasNextPage = currentPageIndex < totalpage;

            // 呈现分页信息
            html.AppendLine(GeneratePagingLink(currentPageIndex > 1, GeneratePagingLinkUrl(context, 1), "第一页"));
            html.AppendLine(GeneratePagingLink(hasPreviousPage, GeneratePagingLinkUrl(context, pageNumber - 1), "前一页"));
            html.AppendLine(string.Format("<span class=\"page-num\">总计 {0} 记录 当前第 {1} 页共 {2} 页</span>",
                totalItemCount, pageNumber, totalpage));
            html.AppendLine(GeneratePagingLink(hasNextPage, GeneratePagingLinkUrl(context, pageNumber + 1), "下一页"));
            html.AppendLine(GeneratePagingLink(currentPageIndex < (totalpage - 1), GeneratePagingLinkUrl(context, totalpage), "最后一页"));

            html.Append("<form method=\"get\">");
            var queryString = context.HttpContext.Request.QueryString;
            foreach (var name in queryString.AllKeys)
            {
                if (!string.Equals(name, "page", StringComparison.CurrentCultureIgnoreCase))
                    html.Append(htmlHelper.Hidden(name, queryString[name]).ToString());
            }
            html.Append(" 跳至第 ");
            html.Append(htmlHelper.TextBox(PAGE_INDEX_NAME, pageNumber, new { @class = "gotoBox", style = "width:30px" }).ToString());
            html.Append(" 页 <input type=\"submit\" value=\"Go\" /></form>");
            return MvcHtmlString.Create(html.ToString());
        }


        public static MvcHtmlString NumericPager(this HtmlHelper htmlHelper, PagingModel pagedEntity)
        {
            return GenerateNumericPagerHtml(htmlHelper, pagedEntity);
        }

        private static MvcHtmlString GenerateNumericPagerHtml(HtmlHelper html, PagingModel pagedEntity)
        {
            if (pagedEntity == null)
                throw new ArgumentNullException("pagedEntity");

            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            int currentPage = pagedEntity.PageIndex; //当前页
            var totalPages = Math.Max((pagedEntity.RecordCount + pagedEntity.PageSize - 1) / pagedEntity.PageSize, 1); //总页数
            var dict = new RouteValueDictionary(html.ViewContext.RouteData.Values);

            var output = new StringBuilder();

            foreach (string key in queryString.Keys)
                if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    dict[key] = queryString[key];

            const string emptyLinkFormat = "<a href=\"javascript:;\">{0}</a>";
            if (totalPages > 1)
            {
                output.Append("<div class=\"pagination pagination-right\"><ul>");

                if (currentPage != 1)
                {
                    //处理首页连接
                    dict[PAGE_INDEX_NAME] = 1;
                    AppendLinkItem(output, html.RouteLink("首页", dict).ToString());
                }
                if (currentPage > 1)
                {
                    //处理上一页的连接
                    dict[PAGE_INDEX_NAME] = currentPage - 1;
                    AppendLinkItem(output, html.RouteLink("上一页", dict).ToString());
                }
                else
                {
                    AppendLinkItem(output, string.Format(emptyLinkFormat, "上一页"), "disabled");
                }

                const int showPageIndex = 10;
                int lowOrderIndex = currentPage - showPageIndex / 2 + 1;
                int upOrderIndex = currentPage + showPageIndex / 2;
                int lowBoundPage = Math.Max(1, lowOrderIndex);
                int upBoundPage = Math.Min(totalPages, upOrderIndex);
                if (lowOrderIndex < 1)
                {
                    upBoundPage = Math.Min(totalPages, upBoundPage + (showPageIndex / 2 - currentPage));
                }
                if (upOrderIndex > totalPages)
                {
                    lowBoundPage = Math.Max(1, lowBoundPage - showPageIndex / 2);
                }



                for (int i = lowBoundPage; i <= upBoundPage; i++)
                {
                    if (i == currentPage)
                    {
                        AppendLinkItem(output, string.Format(emptyLinkFormat, currentPage.ToString()), "active");
                    }
                    else
                    {
                        dict[PAGE_INDEX_NAME] = i;
                        AppendLinkItem(output, html.RouteLink(i.ToString(), dict).ToString());
                    }
                }

                if (currentPage < totalPages)
                {
                    //处理下一页的链接
                    dict[PAGE_INDEX_NAME] = currentPage + 1;
                    AppendLinkItem(output, html.RouteLink("下一页", dict).ToString());
                }
                else
                {
                    AppendLinkItem(output, string.Format(emptyLinkFormat, "下一页"), "disabled");
                }
                if (currentPage != totalPages)
                {
                    dict[PAGE_INDEX_NAME] = totalPages;
                    AppendLinkItem(output, html.RouteLink("末页", dict).ToString());
                }
                output.Append("</ul>");
                output.AppendFormat("<div class=\"pull-left\">第 {0}/{1} 页，每页 {2} 条，共 {3} 条</div>", currentPage,
                                    totalPages, pagedEntity.PageSize,
                                    pagedEntity.RecordCount);
                output.Append("</div>");
            }

            return MvcHtmlString.Create(output.ToString());
        }

        private static void AppendLinkItem(StringBuilder output, string link, string className = "")
        {
            output.AppendFormat("<li{0}>{1}</li>",
                                string.IsNullOrWhiteSpace(className) ? "" : " class=\"" + className + "\"", link);
        }

        private static string GeneratePagingLink(bool hasLink, string linkUrl, string linkText)
        {
            if (hasLink)
                return string.Format("<a href=\"{0}\">{1}</a>", linkUrl, linkText);
            else
                return string.Format("<span class=\"nopaging\">{0}</span>", linkText);
        }

        private static string GeneratePagingLinkUrl(RequestContext context, int page)
        {
            return RequestUrlUtils.CombinationRequestUrl(context, PAGE_INDEX_NAME, page.ToString());
        }
    }
}
